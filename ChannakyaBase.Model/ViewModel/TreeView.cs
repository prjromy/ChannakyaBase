using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChannakyaBase.Model.ViewModel
{
    public class LocationTreeDTO
    {
        public LocationTreeDTO()
        {
            PId = null;
            Image = null;
            IsGroup = true;
            IsChecked = false;
        }
        public int Id { get; set; }
        public Nullable<int> PId { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public bool IsGroup { get; set; }
        public bool IsChecked { get; set; }
        public List<ViewModel.LocationTreeDTO> Children { get; set; }
    }
    public class LocationTreeView
    {

        public LocationTreeView()
        {
            TreeData = new List<ViewModel.LocationTreeDTO>();
            Title = "Treeview";

        }
        public List<ViewModel.LocationTreeDTO> TreeData { get; set; }
        public string Title { get; set; }



    }
}