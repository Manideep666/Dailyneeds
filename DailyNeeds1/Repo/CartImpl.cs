using DailyNeeds1.Entities;
using DailyNeeds1.Repo;
using DailyNeeds1.DTO;
using System.Collections.Generic;
using System.Linq;

namespace DailyNeeds1.Repo
{
    public class CartImpl : IRepo<CartDTO>
    {
        private readonly DailyNeedsDbContext _context;

        public CartImpl(DailyNeedsDbContext context)
        {
            _context = context;
        }

        public bool Add(CartDTO item)
        {
            Cart cartNew = new Cart();
            cartNew.UserID = item.UserID;
            cartNew.ProductID = item.ProductID;
            cartNew.Quantity = item.Quantity;
            _context.cartss.Add(cartNew);
            // _context.SaveChanges();
            return true;
        }

        public bool Delete(int Id)
        { 
            var cartDelete = _context.cartss.Find(Id);

            if (cartDelete == null)
                return false;

            _context.cartss.Remove(cartDelete);
            // _context.SaveChanges();
            return true;
        }

        public List<CartDTO> GetAll()
        {
            var result = _context.cartss
                .Select(c => new CartDTO()
                {
                    CartID = c.CartID,
                    UserID = c.UserID,
                    ProductID = c.ProductID,
                    Quantity = c.Quantity
                    
                })
                .ToList();
            return result;
        }

        public bool Update(CartDTO item)
        {
            var cartUpdate = _context.cartss.Find(item.CartID);

            if (cartUpdate == null)
                return false;

            cartUpdate.UserID = item.UserID;
            cartUpdate.ProductID = item.ProductID;
            cartUpdate.Quantity = item.Quantity;
            // _context.SaveChanges();

            return true;
        }
    }
}
