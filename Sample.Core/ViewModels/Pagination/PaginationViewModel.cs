using Sample.Core.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.ViewModels
{
    public class PaginationViewModel<T> : BaseViewModel
    {
        private List<T> _items;

        public IEnumerable<T> Items
        {
            get
            {
                return _items;
            }
        }

        public PaginationStateViewModel Pagination { get; set; }

        public PaginationViewModel(PaginationViewModel<T> paginationViewModel)
        {
            _items = paginationViewModel._items;
            Pagination = paginationViewModel.Pagination;
        }

        public PaginationViewModel()
        {
            _items = new List<T>();
        }

        public static PaginationViewModel<T> Build(IEnumerable<T> items)
        {
            var paginationViewModel = new PaginationViewModel<T>
            {
                _items = items.ToList()
            };

            paginationViewModel.SetPaginationState(items.Count(), 1, items.Count());

            return paginationViewModel;
        }

        public static PaginationViewModel<T> Build(IEnumerable<T> items, ApiParameters apiParameters, int totalCount)
        {
            var paginationViewModel = new PaginationViewModel<T>
            {
                _items = items.ToList()
            };

            paginationViewModel.SetPaginationState(totalCount, apiParameters);

            return paginationViewModel;
        }

        public void AddItem(T item)
        {
            _items.Add(item);
        }

        public void AddItems(IEnumerable<T> items)
        {
            _items.AddRange(items);
        }

        public bool AnyItems()
        {
            if (Items != null)
            {
                return Items.Any();
            }

            return false;
        }

        public void ReplaceItems(IEnumerable<T> items)
        {
            _items = items.ToList();
        }

        public void SetPaginationState(int totalCount, int page, int limit)
        {
            Pagination = new PaginationStateViewModel
            {
                Count = totalCount,
                Limit = limit,
                Page = page
            };
        }

        public void SetPaginationState(int totalCount, ApiParameters apiParameters)
        {
            Pagination = new PaginationStateViewModel
            {
                Count = totalCount
            };

            if (apiParameters != null)
            {
                Pagination.Page = apiParameters.Page;
                Pagination.Limit = apiParameters.Limit;
            }
        }

        public PaginationViewModel<E> Mutate<E>(Func<T, E> mutator)
        {
            var paginationViewModel = new PaginationViewModel<E>
            {
                Pagination = Pagination
            };

            if (_items != null)
            {
                paginationViewModel._items = _items.Select(mutator).ToList();
            }

            return paginationViewModel;
        }
    }
}
