using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DataModul
{
    public partial class Products
    {
        public int CountForMerchandise { get; set; }
    }

    public partial class UnitsProducts
    {
        public string ViewUnits
        {
            get { return Value+" "+ShortName; }
        }
    }

    [MetadataType(typeof(ModelViewMerchandise))]
    public partial class Merchandises
    {
        public int Prise
        {
            get { return this.MerchandiseProducts.Sum(p => p.CountProduct*p.Products.Price); }
        }
        
        public double Rating
        {
            get
            {
                var k=this.Ratings.Count();
                if(k==0)return -1;
                k = 0;
                var s = 0;
                foreach (Ratings p in Ratings)
                {
                    if (p.Value != 0)
                    {
                        k++;
                        s += p.Value;
                    }
                }
                if (k == 0) return 0;
                return Math.Round((double) (s/k),1);
            }
        }

        public string Сomposition
        {
            get
            {
                string s = "";
                foreach (MerchandiseProducts item in this.MerchandiseProducts)
                {
                    s += item.Products.Name + "(" + item.Products.UnitsProducts.Value + " " + item.Products.UnitsProducts.ShortName + ")" + " x" + item.CountProduct + ";  ";
                }
                return s;
            }
        }

        public int CountOrderes
        {
            get {return this.Orders.Sum(p => p.Count); }
        }
    }

    class ModelViewMerchandise
    {
        [Display(Name = "Название кофе")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        public int Prise { get; set; }

        [Display(Name = "Рейтинг")]
        public double Rating { get; set; }

        
    }


    [MetadataType(typeof(ModelViewOrders))]
    public partial class Orders
    {
        
    }

    class ModelViewOrders
    {
        [Display(Name = "Колличество")]
        public int Count { get; set; }
    }

    [MetadataType(typeof(ModelViewRatings))]
   public partial class Ratings
   {
       
   }

    class ModelViewRatings
    {
        [Display(Name = "Коментарии")]
        [DataType(DataType.MultilineText)]
        public int Comment{ get; set; }
    }
}
