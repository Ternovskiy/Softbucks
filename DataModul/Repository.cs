using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DataModul
{
    public class Repository : IRepository
    {
        public Repository(DataClassesSoftbucksDataContext db)
        {
            Db = db;
        }

        public DataClassesSoftbucksDataContext Db { get; set; }

        #region Product

        public IEnumerable<TypeProducts> GetAllTypeProducts
        {
            get { return Db.TypeProducts; }
        }

        public IEnumerable<Products> GetProductInType(int type)
        {
            return type != -1 ? Db.Products.Where(p => p.TypeProductId == type) : Db.Products;
        }

        #endregion


        #region Merchandise

        public IEnumerable<Products> GetUserProductForMerchandise(string userId)
        {
            int merch = Db.Merchandises
                .Where(p => p.MerchandiseStates.Any(s => s.StateId == 3))
                .Where(p => p.UserId == userId).Select(p => p.Id).FirstOrDefault();

            var prod = Db.MerchandiseProducts.Where(p => p.MerchandiseId == merch).Select(pr => pr.Products);

            foreach (Products item in prod)
            {
                item.CountForMerchandise = item.MerchandiseProducts.First(f => f.MerchandiseId == merch).CountProduct;
            }

            return prod;
        }

        public bool UserProductForMerchandiseAdd(string userId, int productId)
        {

            //var merch = Db.Merchandises
            //    .Where(p => p.MerchandiseStates.Any(s => s.StateId == 3)).First(p => p.UserId == userId);
            int id = 0;
            var st = Db.MerchandiseStates.Where(p => p.StateId == 3);
            if (st.Any())
            {
                var merchs = st.Select(p => p.Merchandises);
                var merch = merchs.Where(p => p.UserId == userId);

                if (!merch.Any())
                {
                    Db.Merchandises.InsertOnSubmit(new Merchandises()
                    {
                        UserId = userId,
                        Name = "New"
                    });
                    Db.Merchandises.Context.SubmitChanges();

                    var k = Db.Merchandises.Max(p => p.Id);
                    Db.MerchandiseStates.InsertOnSubmit(new MerchandiseStates()
                    {
                        MerchandiseId = k,
                        StateId = 3
                    });
                    Db.MerchandiseStates.Context.SubmitChanges();

                    Db.MerchandiseProducts.InsertOnSubmit(new MerchandiseProducts()
                    {
                        CountProduct = 1,
                        MerchandiseId = k,
                        ProductId = productId
                    });
                    Db.MerchandiseProducts.Context.SubmitChanges();
                    return true;
                }
                id = merch.First().Id;
            }
            else
            {
                Db.Merchandises.InsertOnSubmit(new Merchandises()
                {
                    UserId = userId,
                    Name = "Мой кофе(" + DateTime.Now + ")"
                });
                Db.Merchandises.Context.SubmitChanges();

                var k = Db.Merchandises.Max(p => p.Id);
                Db.MerchandiseStates.InsertOnSubmit(new MerchandiseStates()
                {
                    MerchandiseId = k,
                    StateId = 3
                });
                Db.MerchandiseStates.Context.SubmitChanges();

                Db.MerchandiseProducts.InsertOnSubmit(new MerchandiseProducts()
                {
                    CountProduct = 1,
                    MerchandiseId = k,
                    ProductId = productId
                });
                Db.MerchandiseProducts.Context.SubmitChanges();
                return true;
            }
            var mp = Db.MerchandiseProducts.Where(p => p.ProductId == productId && p.MerchandiseId == id);
            if (mp.Count() == 0)
            {
                Db.MerchandiseProducts.InsertOnSubmit(new MerchandiseProducts()
                {
                    CountProduct = 1,
                    MerchandiseId = id,
                    ProductId = productId
                });
            }
            else
            {
                mp.First().CountProduct++;
            }

            Db.MerchandiseProducts.Context.SubmitChanges();
            return true;
        }

        public bool UserProductForMerchandiseKick(string userId, int productId)
        {
            int id = 0;
            var st = Db.MerchandiseStates.Where(p => p.StateId == 3);
            if (st.Any())
            {
                var merchs = st.Select(p => p.Merchandises);
                var merch = merchs.Where(p => p.UserId == userId);

                if (!merch.Any())
                {
                    return false;
                }
                id = merch.First().Id;
            }
            var mp = Db.MerchandiseProducts.Where(p => p.ProductId == productId && p.MerchandiseId == id);
            if (mp.Count() == 0)
            {
                return false;
            }

            var mp1 = mp.First();

            if (mp1.CountProduct == 1)
            {
                Db.MerchandiseProducts.DeleteOnSubmit(mp1);
            }
            else
            {
                mp1.CountProduct--;
            }

            Db.MerchandiseProducts.Context.SubmitChanges();
            return true;
        }

        public bool UserProductForMerchandiseKickAll(string userId)
        {
            int id = 0;
            var st = Db.MerchandiseStates.Where(p => p.StateId == 3);
            if (st.Any())
            {
                var merchs = st.Select(p => p.Merchandises);
                var merch = merchs.Where(p => p.UserId == userId);

                if (!merch.Any())
                {
                    return false;
                }
                id = merch.First().Id;
            }
            var mp = Db.MerchandiseProducts.Where(p => p.MerchandiseId == id);
            if (mp.Count() == 0)
            {
                return false;
            }

            foreach (MerchandiseProducts item in mp)
            {
                Db.MerchandiseProducts.DeleteOnSubmit(item);
            }
            Db.MerchandiseProducts.Context.SubmitChanges();
            return true;
        }



        #endregion


        #region Order

        public bool AddMerchandiseForUserOrder(string userId, int merchandiseId)
        {
            var ms = Db.MerchandiseStates.FirstOrDefault(p => p.StateId == 3 && p.MerchandiseId == merchandiseId);
            if (ms != null)
            {
                Db.MerchandiseStates.DeleteOnSubmit(ms);
                //ms.StateId = 6;
                Db.MerchandiseStates.InsertOnSubmit(new MerchandiseStates()
                {
                    MerchandiseId = merchandiseId,
                    StateId = 6
                });
                Db.MerchandiseStates.Context.SubmitChanges();
            }

            var st =
                Db.OrdersStates.Where(
                    p => p.StateId == 4 && p.Orders.UserId == userId && p.Orders.MerchandaseId == merchandiseId);
            if (st.Any())
            {
                var ord = st.Select(p => p.Orders).First();
                ord.Count++;
                Db.Orders.Context.SubmitChanges();
                return true;
            }
            Db.Orders.InsertOnSubmit(new Orders()
            {
                UserId = userId,
                Count = 1,
                Date = DateTime.Now,
                MerchandaseId = merchandiseId
            });
            Db.Orders.Context.SubmitChanges();

            var idOrd = Db.Orders.Max(p => p.Id);
            Db.OrdersStates.InsertOnSubmit(new OrdersStates()
            {
                OrderId = idOrd,
                StateId = 4
            });
            Db.OrdersStates.Context.SubmitChanges();
            return true;
        }

        public bool AddMyMerchandiseForUserOrder(string userId, string nameMerch)
        {
            var id = Db.MerchandiseStates.Where(p => p.StateId == 3 && p.Merchandises.UserId == userId).First();
            if (id == null) return false;
            if (nameMerch != "")
            {
                id.Merchandises.Name = nameMerch;
                Db.Merchandises.Context.SubmitChanges();
            }
            return AddMerchandiseForUserOrder(userId, id.MerchandiseId);
        }

        public IEnumerable<Orders> GetOrderUser(string userId)
        {
            return Db.Orders.Where(p => p.UserId == userId && p.OrdersStates.Any(s => s.StateId == 4));
        }

        public bool AddMerchandiseToOrder(int orderId)
        {
            try
            {
                Db.Orders.First(p => p.Id == orderId).Count++;
                Db.Orders.Context.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool KickMerchandiseToOrder(int orderId)
        {
            var o=Db.Orders.First(p => p.Id == orderId);
            if(o==null)return false;
                if (o.Count==1)
                {
                    Db.OrdersStates.DeleteAllOnSubmit(Db.OrdersStates.Where(p=>p.OrderId==o.Id));
                    Db.OrdersStates.Context.SubmitChanges();
                    Db.Orders.DeleteOnSubmit(o);
                }
                else
                {
                    o.Count--;
                }
                Db.Orders.Context.SubmitChanges();
                return true;
        }

        public bool BuyOrderUser(string userId)
        {
            var so=Db.OrdersStates.Where(p => p.StateId == 4 && p.Orders.UserId == userId);

            foreach (OrdersStates item in so)
            {
                Db.OrdersStates.InsertOnSubmit(new OrdersStates
                {
                    OrderId = item.OrderId,
                    StateId = 5
                });
            }

            Db.OrdersStates.DeleteAllOnSubmit(so);

            

            Db.OrdersStates.Context.SubmitChanges();
            return false;
        }

        
        #endregion

        #region Price

        public IEnumerable<Merchandises> GetPrice(string name, int pageSize, int page, ref int countPage)
        {
            var pr = Db.Merchandises.Where(p => p.MerchandiseStates.Any(s => s.StateId == 1));
            if (name!="")
            {
                pr = pr.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }

            countPage = pr.Count();
            pr = pr.Skip(pageSize*(page - 1)).Take(pageSize);
            

            return pr;
            //return Db.Merchandises
            //    .Where(p => p.MerchandiseStates.Any(s => s.StateId == 1))
            //    .Where(p => name != "" && p.Name.ToLower().Contains(name.ToLower()))
            //    .Skip(pageSize*(page-1))
            //    .Take(page);
        }

        public Merchandises GerMerchandise(int idMerch)
        {
            return Db.Merchandises.First(p => p.Id == idMerch);
        }

        public IEnumerable<Orders> GetOrderedUser(string userId)
        {
            return Db.Orders.Where(p => p.UserId == userId && p.OrdersStates.Any(s => s.StateId == 5))
                .OrderByDescending(o=>o.Date);
        }

        public bool SetRating(string userId, int idMerch, int value, string comment)
        {
            var r = Db.Ratings.FirstOrDefault(p => p.MerchandiseId == idMerch && p.UserId == userId);
            if (r!=null)
            {
                r.Comment = comment;
                r.Value = value;
            }
            else
            {
                Db.Ratings.InsertOnSubmit(new Ratings()
                {
                    UserId = userId,MerchandiseId = idMerch,Value = value,Comment = comment
                });
            }
            Db.Ratings.Context.SubmitChanges();
            return true;
        }

        public IEnumerable<Ratings> GetCommentForMerch(int merchId)
        {
            return Db.Ratings.Where(p => p.MerchandiseId == merchId);
        }

        public Ratings GetRatingsForUser(string userId, int merchId)
        {
            return Db.Ratings.FirstOrDefault(p => p.UserId == userId && p.MerchandiseId == merchId);
        }

        #endregion

    }
}
