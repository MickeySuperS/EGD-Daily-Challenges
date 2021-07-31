using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public class Key
    {
        public Key(int id)
        {
            this._id = id;
        }
        public int id => _id; int _id;
        public Action OnKeyDestroyed;
    }
}
