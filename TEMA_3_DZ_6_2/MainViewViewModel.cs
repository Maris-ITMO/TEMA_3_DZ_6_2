using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Prism.Commands;
using TEMA_3_DZ_6_2_Library;

namespace TEMA_3_DZ_6_2
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<Level> Levels { get; } = new List<Level>(); 
        public List<FamilySymbol> FurnitureTypes { get; } = new List<FamilySymbol>();
        public DelegateCommand SaveCommand { get; }
        public List<XYZ> Points { get; } = new List<XYZ>();
        public FamilySymbol SelectedFurnitureType { get; set; }
        public Level SelectedLevel { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Levels = LevelsUtils.GetLevels(commandData);
            FurnitureTypes = FamilySumbolUtils.GetFurnitureSymbols(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            Points = SelectionUtils.GetPoints(_commandData, "Выберите точки", ObjectSnapTypes.Endpoints);

        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Points.Count < 1 ||
                SelectedFurnitureType == null ||
                SelectedLevel == null)
                return;

            foreach (var point in Points)
            {
                FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurnitureType, point, SelectedLevel);
            }

            RaiseCloseRequest();
        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

    }
}
