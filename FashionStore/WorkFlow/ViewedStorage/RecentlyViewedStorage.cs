using System.Collections.Generic;

namespace FashionStore.WorkFlow.ViewedStorage
{
   
    public class RecentlyViewedStorage
    {
        private readonly Queue<int> _storage;
        public int Count { get { return _storage.Count; } }
        private short _size;
        public RecentlyViewedStorage(short size)
        {
            _size = size;
            _storage = new Queue<int>(_size);
            
        }
        public void Add(int id)
        {
            if (!_storage.Contains(id))
            {
                _storage.Enqueue(id);
                Dequeue();
            }
        }
        public IEnumerable<int> GetAll()
        {
            return _storage;
        }

        private void Dequeue()
        {
            if (_storage.Count > _size)
            {
                _storage.Dequeue();
            }
        }

    }
}