using System.Collections.Generic;
using System.Linq;

namespace OurTime.WebUI.Models.Cart
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new();

        // Räknar ut totalkostnaden
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);

        // Lägg till en vara i kundkorgen
        public void AddItem(CartItem item)
        {
            var exists = Items.FirstOrDefault(x => x.Id == item.Id);
            if (exists != null)
            {
                // Om varan redan finns, öka antal
                exists.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
    }
}
