﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KVSCommon.Database
{
    /// <summary>
    /// Erweiterugnsklasse für die OrderInvoice Tabelle
    /// </summary>
    public partial class OrderInvoice : ILogging
    {
        public DataClasses1DataContext LogDBContext
        {
            get;
            set;
        }

        public object ItemId
        {
            get
            {
                return this.InvoiceId;
            }
        }

        public EntityState EntityState
        {
            get;
            set;
        }

        /// <summary>
        /// Erstellt eine Verknuepfung zwischen einem Auftrag und einer Rechnung.
        /// </summary>
        /// <param name="orderId">Id des Auftrags.</param>
        /// <param name="invoiceId">Id der Rechnung.</param>
        /// <param name="dbContext">Datenbankkontext für die Transaktion.</param>
        /// <returns>Die neue Verknüpfung.</returns>
        public static OrderInvoice CreateOrderInvoice(Guid orderId, Guid invoiceId, DataClasses1DataContext dbContext)
        {
            OrderInvoice item = new OrderInvoice()
            {
                OrderId = orderId,
                InvoiceId = invoiceId
            };

            dbContext.OrderInvoice.InsertOnSubmit(item);
            ///  var invoiceNumber = dbContext.Invoice.Where(q => q.Id == invoiceId).Select(q => q.InvoiceNumber).Single();
            var orderNumber = dbContext.Order.Where(q => q.Id == orderId).Select(q => q.Ordernumber).Single();
            dbContext.WriteLogItem("Rechnung wurde mit Auftrag " + orderNumber + " verknüpft.", LogTypes.INSERT, orderId, "OrderInvoice", invoiceId);
            return item;
        }
        /// <summary>
        /// Loescht eine Auftrags/Rechnungsverknuepfung
        /// </summary>
        /// <param name="orderId">AuftragsID</param>
        /// <param name="invoiceId">RechnungsID</param>
        /// <param name="dbContext">DB Kontext</param>
        public static void DeleteOrderInvoice(Guid orderId, Guid invoiceId, DataClasses1DataContext dbContext)
        {
            var item = dbContext.OrderInvoice.SingleOrDefault(q => q.OrderId == orderId && q.InvoiceId == invoiceId);
            if (item == null)
            {
                var orderNumber = dbContext.Order.Where(q => q.Id == orderId).Select(q => q.Ordernumber).SingleOrDefault();
                var invoiceNumber = dbContext.Invoice.Where(q => q.Id == invoiceId).Select(q => q.InvoiceNumber.Number).SingleOrDefault();
                throw new Exception("Es existiert keine Verknüpfung zwischen dem Auftrag Nr. " + orderNumber + " und der Rechnung Nr. " + invoiceNumber);
            }

            dbContext.OrderInvoice.DeleteOnSubmit(item);
            dbContext.WriteLogItem("Verknüpfung zwischen Auftrag Nr. " + item.Order.Ordernumber + " und Rechnung Nr. " + item.Invoice.InvoiceNumber.Number + " wurde gelöscht.", LogTypes.DELETE, orderId, "OrderInvoice", invoiceId);
        }
        /// <summary>
        /// Aenderungsevents für die Historie
        /// </summary>
        /// <param name="value"></param>
        partial void OnCreated()
        {
            this.EntityState = Database.EntityState.New;
        }
        /// <summary>
        /// Aenderungsevents für die Historie
        /// </summary>
        /// <param name="value"></param>
        partial void OnLoaded()
        {
            this.EntityState = Database.EntityState.Loaded;
        }
    }
}
