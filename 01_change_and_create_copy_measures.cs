using System;
using System.IO;
using System.Linq;
 
var measuresToCopy = "Measure1, Measure2, etc."

string folderL2 = "Rations Layer L2";
 
// Template for L2 
string l2Template = @"
VAR sort = SELECTEDVALUE('Upload_Union'[Sort])
VAR uploadColumn = SELECTEDVALUE('Upload_Union'[Upload])
VAR upload1 = CALCULATE([VALUE_PARAMETR], Dim_Upload[Upload] = SELECTEDVALUE(Slicer_Up_1[Upload]))
VAR upload2 = CALCULATE([VALUE_PARAMETR], Dim_Upload[Upload] = SELECTEDVALUE(Slicer_Up_2[Upload]))
VAR absDiff = upload2 - upload1
VAR relDiff = DIVIDE(upload2 - upload1, upload1)
VAR upload3 = CALCULATE([VALUE_PARAMETR], Dim_Upload[Upload] = SELECTEDVALUE(Slicer_Up_3[Upload]))
VAR upload4 = CALCULATE([VALUE_PARAMETR], Dim_Upload[Upload] = SELECTEDVALUE(Slicer_Up_4[Upload]))
VAR absDiff2 = upload4 - upload3
VAR relDiff2 = DIVIDE(upload4 - upload3, upload3)
VAR columnStructure = SELECTEDVALUE('p_Upload_Structure'[Slicer_Text])
 
RETURN
SWITCH(TRUE(),
    columnStructure = ""Uploads Only"", [VALUE_PARAMETR],
    uploadColumn = ""Abs Diff. (2/1)"", absDiff,
    uploadColumn = ""Rel Diff. (2/1)"", BLANK(),
    AND(uploadColumn = SELECTEDVALUE(Slicer_Up_1[Upload]), sort = ""1""), upload1,
    AND(uploadColumn = SELECTEDVALUE(Slicer_Up_2[Upload]), sort = ""2""), upload2,
    uploadColumn = ""Abs Diff. (4/3)"", absDiff2,
    uploadColumn = ""Rel Diff. (4/3)"", BLANK(),
    AND(uploadColumn = SELECTEDVALUE(Slicer_Up_3[Upload]), sort = ""5""), upload3,
    AND(uploadColumn = SELECTEDVALUE(Slicer_Up_4[Upload]), sort = ""6""), upload4,
    BLANK())
";
 

foreach (var measure in Model.AllMeasures)
{
    
    if (measuresToCopy.Contains(measure.Name))
    {
       
        if (measure.Expression.Contains("[Value]"))
        {
            measure.Expression = measure.Expression.Replace("[Value]", "[Value V1]");
           
        }
 
        // L2 measure
        var newMeasureNameL2 = measure.Name + " L2";
        var newExpressionL2 = l2Template.Replace("VALUE_PARAMETR", measure.Name);
        var newMeasureL2 = measure.Table.AddMeasure(newMeasureNameL2, newExpressionL2);
       
        newMeasureL2.FormatString = measure.FormatString;
        newMeasureL2.Description = measure.Description;
        newMeasureL2.DisplayFolder = folderL2;
        newMeasureL2.IsHidden = measure.IsHidden;
 
    }
}
