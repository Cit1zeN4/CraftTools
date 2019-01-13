using CraftTools.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftTools.Helpers
{
    public class ChangerModel
    {
        public enum WareMaterialStatus
        {
            OnList,
            Added,
            Deleted,
            Changed
        }

        public WareMaterial WareMaterial { get; set; }
        public int WareMaterialId { get; set; }
        public WareMaterialStatus Status { get; set; }
    }

    public class WareMaterialChanger
    {
        public WareMaterialChanger()
        {
            List = new List<ChangerModel>();
        }

        public WareMaterialChanger(ObservableCollection<WareMaterial> item)
        {
            List = new List<ChangerModel>();
            foreach(WareMaterial wm in item)
            {
                List.Add(new ChangerModel { WareMaterialId = wm.WareMaterialId, WareMaterial = wm, Status = ChangerModel.WareMaterialStatus.OnList });
            }
        }

        public List<ChangerModel> List { get; set; }

        public void Add(WareMaterial item)
        {
            if (List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault() == null)
            {
                List.Add(new ChangerModel { WareMaterialId = item.WareMaterialId, WareMaterial = item, Status = ChangerModel.WareMaterialStatus.Added});
            }
            else
            {
                if(List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault().Status == ChangerModel.WareMaterialStatus.Added)
                {
                    List.Add(new ChangerModel { WareMaterialId = item.WareMaterialId, WareMaterial = item, Status = ChangerModel.WareMaterialStatus.Added });
                }
                else
                {
                    ChangerModel model = List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault();
                    model.Status = ChangerModel.WareMaterialStatus.Added;
                }
            }
        }

        public void Delete(WareMaterial item)
        {
            if (List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault() == null)
            {
                List.Add(new ChangerModel { WareMaterialId = item.WareMaterialId, WareMaterial = item, Status = ChangerModel.WareMaterialStatus.Deleted });
            }
            else
            {
                if(List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault().Status == ChangerModel.WareMaterialStatus.Added)
                {
                    List.Remove(List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault());
                }
                else
                {
                    ChangerModel model = List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault();
                    model.Status = ChangerModel.WareMaterialStatus.Deleted;
                }
            }
        }

        public void Change(WareMaterial item)
        {
            if(List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault().Status == ChangerModel.WareMaterialStatus.Added)
            {
                ChangerModel model = List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault();
                model.WareMaterial = item;
            }
            else
            {
                ChangerModel model = List.Where(o => o.WareMaterialId == item.WareMaterialId).FirstOrDefault();
                model.WareMaterial = item;
                model.Status = ChangerModel.WareMaterialStatus.Changed;
            }
        }
    }
}
