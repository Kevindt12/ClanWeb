using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClanWeb.Web.AppCode.UI.Web
{
    public class PageCounter
    {

        private int _itemsPerPage;
        private int _maxPageCount;
        private int _page;
        private List<object> _items;

        
        public int ItemsPerPage
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = value;
            }
        }

        public int MaxPageCount
        {
            get
            {
                return _maxPageCount;
            }
            private set
            {
                _maxPageCount = value;
            }
        }

        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
            }
        }

        public List<object> Items
        {
            get
            {
                return _items;
            }
            private set
            {
                _items = value;
            }
        }
        


        public PageCounter(int page, List<object> items)
        {
            Page = page;
            Items = items;
        }

        public PageCounter(int page, int itemsPerPage, List<object> items)
        {
            Page = page;
            ItemsPerPage = itemsPerPage;
            Items = items;

            // Set what the max items per page are
            MaxPageCount = (int)Math.Ceiling((decimal)Items.Count / ((decimal)ItemsPerPage - 1));

        }



        public List<object> GetObjectForPage()
        {
            List<object> objects = new List<object>();
                 
            // Getting the start value of the count
            int startCount = ItemsPerPage * Page - ItemsPerPage;
            int endCount = ItemsPerPage * Page - 1;

            // Setting the max page count
            MaxPageCount = (int)Math.Ceiling((decimal)Items.Count / ((decimal)ItemsPerPage - 1));

            for (int i = startCount; (i < Items.Count && i <= endCount); i++)
            {
                objects.Add(Items[i]);
            }

            return objects;
        }



        public List<object> GetObjectForPage(int page)
        {
            List<object> objects = new List<object>();

            // Getting the start value of the count
            int startCount = ItemsPerPage * page - ItemsPerPage;
            int endCount = ItemsPerPage * page - 1;

            // Setting the max page count
            MaxPageCount = (int)Math.Ceiling((decimal)Items.Count / ((decimal)ItemsPerPage - 1));

            // Selecing only that part of the list
            for (int i = startCount; (i < Items.Count && i <= endCount); i++)
            {
                objects.Add(Items[i]);
            }

            return objects;
        }






    }
}