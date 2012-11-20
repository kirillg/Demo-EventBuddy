using EventBuddy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace EventBuddy.DataModel
{
    using System.ComponentModel.DataAnnotations;

    public class Event //: BindableBase
    {
        public Event()
        {

            Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9 - Start.Hour, 0, 0);            
            End = Start.AddHours(56);
            Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum nostrud ipsum consectetur.";
        }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "start")]
        public DateTime Start { get; set; }

        [DataMember(Name = "end")]
        public DateTime End { get; set; }

        [Required]
        [DataMember(Name = "description")]
        public string Description { get; set; }
        
        //[DataMember(Name = "token")]
        //public string Token { get; set; }

      /*  [IgnoreDataMember]
        public BitmapImage Image
        {
            get
            {
                return LoadImage().Result;
            }
        }

        public async Task<BitmapImage> LoadImage()
        {        
            BitmapImage image = new BitmapImage();
            if (Token != null)
            {
                var file = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFileAsync(Token);
                if (file != null)
                {
                    using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        image.SetSource(stream);
                    }
                }
            }
            return image;
        }  */

         /*
        private string _description;
        private DateTime _end;
        private int _id;
        private string _name;
        private DateTime _start;

        [DataMember(Name = "id")]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        [DataMember(Name = "start")]
        public DateTime Start
        {
            get { return _start; }
            set { SetProperty(ref _start, value); }
        }

        [DataMember(Name = "end")]
        public DateTime End
        {
            get { return _end; }
            set { SetProperty(ref _end, value); }
        }

        [DataMember(Name = "description")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }   
         * */
    }
}
