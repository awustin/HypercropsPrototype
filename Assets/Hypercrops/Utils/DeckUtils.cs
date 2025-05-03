using UnityEngine;


namespace Assets.Hypercrops.Utils
{
    public static class DeckUtils
    {
        public static Vector3 MapOrderToPosition(int order)
        {
            return new Vector3(order * 180f + 200f, 25f, 0);
        }
    }
}
