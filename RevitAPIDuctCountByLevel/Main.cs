using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIDuctCountByLevel
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var ducts = new FilteredElementCollector(doc)
                .OfClass(typeof(Duct))
                .Cast<Duct>()
                .ToList();

            var ductsOnFirstLevel = new List<Duct>();
            var ductsOnSecondLevel = new List<Duct>();

            foreach (var element in ducts)
            {
                
                if (element.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM).AsValueString()!=null)
                {
                    string level = element.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM).AsValueString();
                    if (level == "Level 1")
                        ductsOnFirstLevel.Add(element);
                    if (level == "Level 2")
                        ductsOnSecondLevel.Add(element);
                }
            }

            TaskDialog.Show("Duct info", $"Количество воздуховодов на 1 этаже: {ductsOnFirstLevel.Count.ToString()}" +
                $"{Environment.NewLine}Количество воздуховодов на 2 этаже: {ductsOnSecondLevel.Count.ToString()}");

            return Result.Succeeded;
        }
    }
}
