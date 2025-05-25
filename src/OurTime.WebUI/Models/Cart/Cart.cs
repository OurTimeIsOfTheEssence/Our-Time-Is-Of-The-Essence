using System.Collections.Generic;
using System.Linq;

namespace OurTime.WebUI.Models.Cart
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new();

        public decimal Total => Items.Sum(i => i.Price * i.Quantity);

        public void AddItem(CartItem item)
        {
            var exists = Items.FirstOrDefault(x => x.Id == item.Id);
            if (exists != null)
            {
                exists.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
    }
}
