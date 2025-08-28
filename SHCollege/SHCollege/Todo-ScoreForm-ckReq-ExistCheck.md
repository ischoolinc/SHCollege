# Todo — ScoreForm.cs（做法 A：寫入前先確認 `SemsSubjDataDict` 有匹配行）

> 參考基準：**Todo-ScoreForm111-ckReq-ExistCheck.md** 的處理方式，將同一套「存在性守門」邏輯套用到 `ScoreForm.cs`。  
> 目的：避免把**主資料 `SemsSubjDataDict` 不存在的科目**寫入輸出欄位。

---

## 修改檔案
- `ScoreForm.cs`

## 變更範圍
- 方法：`_bgExporData_DoWork(object sender, DoWorkEventArgs e)`
- 區塊：`// --- 再次修習成績覆蓋 ---` 中，所有直接寫入 `newRow[...] = retake.RetakeScore` 的地方。

> 本次僅調整「寫入輸出列」行為；**不**更動原本針對 `SemsSubjDataDict` / `retakeSemsSubjDataDict` 的回填（`match["原始成績"]`、`match["補考成績"]`）與後續重算流程。

---

## 具體作法（步驟）

### Step 1：在寫入輸出欄位前加入存在性檢查
在 `foreach (var retake in retakeDict[sid])` 迴圈內，先以既有方式取得年級（如原程式呼叫 `GetGradeYearFromSemsSubjData(...)`）。接著新增「主資料存在性檢查」變數：

```csharp
// 取得年級資訊（依現有邏輯）
string gradeYear = GetGradeYearFromSemsSubjData(retakeSemsSubjDataDict, sid, retake.Subject, retake.Semester, retake.SubjectLevel);

// 先確認主資料中是否存在相同科目/學期/(可選)年級/(可選)科目級別
bool existsInMain = SemsSubjDataDict.ContainsKey(sid) &&
    SemsSubjDataDict[sid].Any(r =>
        r["科目"].ToString().Trim() == retake.Subject &&
        r["學期"].ToString() == retake.Semester &&
        (string.IsNullOrEmpty(gradeYear) || r["成績年級"].ToString() == gradeYear) &&
        (string.IsNullOrEmpty(retake.SubjectLevel) ||
         (r.Table.Columns.Contains("科目級別") && r["科目級別"].ToString() == retake.SubjectLevel))
    );
```

### Step 2：以 `existsInMain` 作為寫入 `newRow` 的前置條件
將原本直接寫入輸出欄位的區塊，外層加上 `if (existsInMain) { ... }` 守門條件：

```csharp
// 使用 retakeScoreNameMappingDict 進行欄位名稱比對
string compositeKey = $"{retake.Subject}({GetGradeSemesterString(gradeYear, retake.Semester)})";

if (existsInMain)
{
    if (retakeScoreNameMappingDict.ContainsKey(compositeKey))
    {
        string actualFieldName = retakeScoreNameMappingDict[compositeKey];
        if (exportDT.Columns.Contains(actualFieldName))
        {
            newRow[actualFieldName] = retake.RetakeScore;
        }
    }
    else
    {
        // 備援：以字串拼接的欄位名
        string colName = $"{retake.Subject}({GetGradeSemesterString(gradeYear, retake.Semester)})";
        if (exportDT.Columns.Contains(colName))
        {
            newRow[colName] = retake.RetakeScore;
        }
    }
}
```

---

## 建議差異片段（請在 `// --- 再次修習成績覆蓋 ---` 內第一個寫入點套用）
```diff
 foreach (var retake in retakeDict[sid])
 {
     string gradeYear = GetGradeYearFromSemsSubjData(retakeSemsSubjDataDict, sid, retake.Subject, retake.Semester, retake.SubjectLevel);

-    // 使用 retakeScoreNameMappingDict 進行欄位名稱比對
-    string compositeKey = $"{retake.Subject}({GetGradeSemesterString(gradeYear, retake.Semester)})";
+    // Step A：主資料存在性檢查
+    bool existsInMain = SemsSubjDataDict.ContainsKey(sid) &&
+        SemsSubjDataDict[sid].Any(r =>
+            r["科目"].ToString().Trim() == retake.Subject &&
+            r["學期"].ToString() == retake.Semester &&
+            (string.IsNullOrEmpty(gradeYear) || r["成績年級"].ToString() == gradeYear) &&
+            (string.IsNullOrEmpty(retake.SubjectLevel) ||
+             (r.Table.Columns.Contains("科目級別") && r["科目級別"].ToString() == retake.SubjectLevel))
+        );
+
+    // 使用 retakeScoreNameMappingDict 進行欄位名稱比對（僅在 existsInMain 時寫入）
+    string compositeKey = $"{retake.Subject}({GetGradeSemesterString(gradeYear, retake.Semester)})";

-    if (retakeScoreNameMappingDict.ContainsKey(compositeKey))
+    if (existsInMain && retakeScoreNameMappingDict.ContainsKey(compositeKey))
     {
         string actualFieldName = retakeScoreNameMappingDict[compositeKey];
         if (exportDT.Columns.Contains(actualFieldName))
         {
             newRow[actualFieldName] = retake.RetakeScore;
         }
     }
     else
     {
         // 備援：以字串拼接的欄位名
         string colName = $"{retake.Subject}({GetGradeSemesterString(gradeYear, retake.Semester)})";
-        if (exportDT.Columns.Contains(colName))
+        if (existsInMain && exportDT.Columns.Contains(colName))
         {
             newRow[colName] = retake.RetakeScore;
         }
     }
 }
```

---

## 驗收重點（Acceptance Criteria）
1. 當某科目**不在** `SemsSubjDataDict[sid]` 中時：對應欄位 **不得**被寫入 retake 分數（維持 `-1` 或原值）。
2. 當某科目**存在**於 `SemsSubjDataDict[sid]` 中（符合科目、學期，必要時年級／科目級別）：仍正常寫入 `retake.RetakeScore` 至輸出欄位。
3. 其他流程（回填 `match["原始成績"]`／`["補考成績"]`、分項重算等）保持不變。

## 測試建議
- 學生甲：某科目只在 `retakeSemsSubjDataDict` 出現，主資料沒有 → 輸出欄位不應寫入。
- 學生乙：某科目在兩邊都存在 → 輸出欄位應寫入。
- 若系統使用「科目級別」欄位：同名不同級別時，只允許正確級別寫入。

---

> 延伸建議（可另案）：若要讓 `retakeSemsSubjDataDict` 與主資料取得條件一致（例如也跟隨 `ckReq.Checked`），可在本次修改後續追加調整，以維持來源一致性。
