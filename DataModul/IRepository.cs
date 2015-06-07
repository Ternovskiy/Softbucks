using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModul
{
    public interface IRepository
    {
        DataClassesSoftbucksDataContext Db { get; set; }

        IEnumerable<TypeProducts> GetAllTypeProducts { get; }

        IEnumerable<Products> GetProductInType(int type);

        IEnumerable<Products> GetUserProductForMerchandise(string userId);

        bool UserProductForMerchandiseAdd(string userId, int productId);

        bool UserProductForMerchandiseKick(string userId, int productId);

        bool UserProductForMerchandiseKickAll(string userId);

        bool AddMerchandiseForUserOrder(string userId, int merchandiseId);

        bool AddMyMerchandiseForUserOrder(string userId,string nameMerch);

        IEnumerable<Orders> GetOrderUser(string userId);

        bool AddMerchandiseToOrder(int orderId);

        bool KickMerchandiseToOrder(int orderId);

        bool BuyOrderUser(string userId);

        IEnumerable<Merchandises> GetPrice(string name, int pageSize, int page, ref int countPage);

        Merchandises GerMerchandise(int idMerch);

        IEnumerable<Orders> GetOrderedUser(string userId);

        bool SetRating(string userId, int idMerch, int value, string comment);

        IEnumerable<Ratings> GetCommentForMerch(int merchId);

        Ratings GetRatingsForUser(string userId, int merchId);
    }
}
