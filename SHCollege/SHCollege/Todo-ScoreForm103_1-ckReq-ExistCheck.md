# Todo — ScoreForm103_1.cs（做法 A：寫入前先確認 `SemsSubjDataDict` 有匹配行）

> 參考基準：**Todo-ScoreForm111-ckReq-ExistCheck.md** 的處理方式，等同邏輯套用在 `ScoreForm103_1.cs`。  
> 目的：避免把「主資料 `SemsSubjDataDict` 不存在的科目」寫入輸出欄位。

---

## 修改檔案
- `ScoreForm103_1.cs`

## 變更範圍
- 方法：`_bgExporData_DoWork(object sender, DoWorkEventArgs e)`
- 區塊：`// --- 再次修習成績覆蓋 ---` 內部，所有直接寫入 `newRow[...] = retake.RetakeScore` 的位置。

---

## 具體作法（步驟）

### Step 1：在寫入輸出欄位前加入存在性檢查
在迴圈（`foreach (var retake in retakeDict[sid])`）中，先以既有的方式取得年級：
```csharp
string gradeYear = GetGradeYearFromSemsSubjData(retakeSemsSubjDataDict, sid, retake.Subject, retake.Semester, retake.SubjectLevel);
```

緊接著新增「主資料存在性檢查」變數：
```csharp
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
將原本直接寫入輸出欄位的區塊：
```csharp
// 使用 retakeScoreNameMappingDict 進行欄位名稱比對
string compositeKey = $"{retake.Subject}({GetGradeSemesterString(gradeYear, retake.Semester)})";

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
```

改成加上守門條件：
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

> 備註：此變更**僅限**「寫入輸出列」的行為；不更動原本針對 `SemsSubjDataDict` 或 `retakeSemsSubjDataDict` 的回填（`match["原始成績"]`、`match["補考成績"]`）與後續重算流程。

---

## 參考差異片段（請在 `// --- 再次修習成績覆蓋 ---` 內的第一個寫入點套用）
```diff
 foreach (var retake in retakeDict[sid])
 {
     string gradeYear = GetGradeYearFromSemsSubjData(retakeSemsSubjDataDict, sid, retake.Subject, retake.Semester, retake.SubjectLevel);
 
-    // 使用 retakeScoreNameMappingDict 進行欄位名稱比對
-    string compositeKey = $"{retake.Subject}({GetGradeSemesterString(gradeYear, retake.Semester)})";
+    // 先確認主資料中是否存在相同科目/學期/(可選)年級/(可選)科目級別
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
         // 如果對照字典中沒有找到，則使用原本的字串拼接方式作為備用
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
1. 當某科目**不在** `SemsSubjDataDict[sid]` 中時：
   - 對應欄位 **不得**被寫入 retake 分數（維持 `-1` 或原值）。
2. 當某科目**存在**於 `SemsSubjDataDict[sid]` 中（符合科目、學期，必要時年級／科目級別）：
   - 仍正常寫入 `retake.RetakeScore` 至輸出欄位。
3. 其他流程（回填 `match["原始成績"]`／`["補考成績"]`、分項重算等）保持不變。

## 測試建議
- 準備一位學生，其中某科目（例：「化學(高二下)」）**只出現在 `retakeSemsSubjDataDict`，不在 `SemsSubjDataDict`**：
  - 產出欄位 **不應**被寫入 retake 分數。
- 準備一位學生，其中「物理(高二上)」**兩邊都存在**：
  - 產出欄位 **應**寫入 retake 分數。
- 若系統使用「科目級別」欄位：
  - 測試同名不同級別時，只允許對**正確級別**寫入。

---

> 若未來需要擴充為「找不到主資料時自動新增該科」的行為，請另開需求，需同時修正輸出與分項重算兩處，避免計算不一致。
