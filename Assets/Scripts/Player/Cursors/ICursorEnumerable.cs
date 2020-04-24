using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Items.MonoItems;

namespace Player.PlayerCursors
{
    public interface ICursorEnumerable
    {
        event Action OnCollectionChanged;

        List<MonoItem> GetCollection();
    }
}
