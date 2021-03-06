﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVSWebApplication.Abrechnung
{
    public class SelectedInvoiceItems 
    {
        private string productName;

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        private string amount;
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private Guid orderItemId;
        public Guid OrderItemId
        {
            get { return orderItemId; }
            set { orderItemId = value; }
        }
     
        private Guid? costCenterId;
        public Guid? CostCenterId
        {
            get { return costCenterId; }
            set { costCenterId = value; }
        }
        private Guid orderId;
        public Guid OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        private int itemCount;
        public int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
        }
        private Guid? orderLocationId;
        public Guid? OrderLocationId
        {
            get { return orderLocationId; }
            set { orderLocationId = value; }
        }
        private string orderLocationName;
        public string OrderLocationName
        {
            get { return orderLocationName; }
            set { orderLocationName = value; }
        }
    }
}