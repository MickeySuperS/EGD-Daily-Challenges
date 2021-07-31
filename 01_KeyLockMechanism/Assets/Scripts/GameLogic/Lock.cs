using System;

namespace EGD
{
    public class Lock
    {
        public Lock(int id, int numberOfKeysRequired)
        {
            this._id = id;
            this._numberOfKeysRequired = numberOfKeysRequired;
        }
        public int id => _id; int _id;
        public int numberOfKeysRequired => _numberOfKeysRequired; int _numberOfKeysRequired;
        public bool isLocked => _numberOfKeysRequired > 0;

        public Action OnKeyUsed;
        public Action OnLockDestroyed;

        public bool UseKey(Key key)
        {
            if (key.id != _id) return false;

            _numberOfKeysRequired--;
            OnKeyUsed?.Invoke();
            return true;
        }

    }
}