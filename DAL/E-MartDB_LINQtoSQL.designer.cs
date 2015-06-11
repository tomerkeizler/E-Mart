﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="E-Mart")]
	public partial class E_MartDB_LINQtoSQLDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertProduct(Product instance);
    partial void UpdateProduct(Product instance);
    partial void DeleteProduct(Product instance);
    partial void InsertTopSeller(TopSeller instance);
    partial void UpdateTopSeller(TopSeller instance);
    partial void DeleteTopSeller(TopSeller instance);
    #endregion
		
		public E_MartDB_LINQtoSQLDataContext() : 
				base(global::DAL.Properties.Settings.Default.E_MartConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public E_MartDB_LINQtoSQLDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public E_MartDB_LINQtoSQLDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public E_MartDB_LINQtoSQLDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public E_MartDB_LINQtoSQLDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Product> Products
		{
			get
			{
				return this.GetTable<Product>();
			}
		}
		
		public System.Data.Linq.Table<TopSeller> TopSellers
		{
			get
			{
				return this.GetTable<TopSeller>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Products")]
	public partial class Product : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _Name;
		
		private int _Type;
		
		private int _Price;
		
		private int _StockCount;
		
		private int _InStock;
		
		private int _Location;
		
		private int _ProductID;
		
		private EntityRef<TopSeller> _TopSeller;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnTypeChanging(int value);
    partial void OnTypeChanged();
    partial void OnPriceChanging(int value);
    partial void OnPriceChanged();
    partial void OnStockCountChanging(int value);
    partial void OnStockCountChanged();
    partial void OnInStockChanging(int value);
    partial void OnInStockChanged();
    partial void OnLocationChanging(int value);
    partial void OnLocationChanged();
    partial void OnProductIDChanging(int value);
    partial void OnProductIDChanged();
    #endregion
		
		public Product()
		{
			this._TopSeller = default(EntityRef<TopSeller>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="Int NOT NULL")]
		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Price", DbType="Int NOT NULL")]
		public int Price
		{
			get
			{
				return this._Price;
			}
			set
			{
				if ((this._Price != value))
				{
					this.OnPriceChanging(value);
					this.SendPropertyChanging();
					this._Price = value;
					this.SendPropertyChanged("Price");
					this.OnPriceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StockCount", DbType="Int NOT NULL")]
		public int StockCount
		{
			get
			{
				return this._StockCount;
			}
			set
			{
				if ((this._StockCount != value))
				{
					this.OnStockCountChanging(value);
					this.SendPropertyChanging();
					this._StockCount = value;
					this.SendPropertyChanged("StockCount");
					this.OnStockCountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InStock", DbType="Int NOT NULL")]
		public int InStock
		{
			get
			{
				return this._InStock;
			}
			set
			{
				if ((this._InStock != value))
				{
					this.OnInStockChanging(value);
					this.SendPropertyChanging();
					this._InStock = value;
					this.SendPropertyChanged("InStock");
					this.OnInStockChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Location", DbType="Int NOT NULL")]
		public int Location
		{
			get
			{
				return this._Location;
			}
			set
			{
				if ((this._Location != value))
				{
					this.OnLocationChanging(value);
					this.SendPropertyChanging();
					this._Location = value;
					this.SendPropertyChanged("Location");
					this.OnLocationChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int ProductID
		{
			get
			{
				return this._ProductID;
			}
			set
			{
				if ((this._ProductID != value))
				{
					this.OnProductIDChanging(value);
					this.SendPropertyChanging();
					this._ProductID = value;
					this.SendPropertyChanged("ProductID");
					this.OnProductIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Product_TopSeller", Storage="_TopSeller", ThisKey="ProductID", OtherKey="ProductID", IsUnique=true, IsForeignKey=false)]
		public TopSeller TopSeller
		{
			get
			{
				return this._TopSeller.Entity;
			}
			set
			{
				TopSeller previousValue = this._TopSeller.Entity;
				if (((previousValue != value) 
							|| (this._TopSeller.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._TopSeller.Entity = null;
						previousValue.Product = null;
					}
					this._TopSeller.Entity = value;
					if ((value != null))
					{
						value.Product = this;
					}
					this.SendPropertyChanged("TopSeller");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TopSellers")]
	public partial class TopSeller : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ProductID;
		
		private bool _IsTopSeller;
		
		private int _SellCounter;
		
		private int _CurrentMonth;
		
		private EntityRef<Product> _Product;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnProductIDChanging(int value);
    partial void OnProductIDChanged();
    partial void OnIsTopSellerChanging(bool value);
    partial void OnIsTopSellerChanged();
    partial void OnSellCounterChanging(int value);
    partial void OnSellCounterChanged();
    partial void OnCurrentMonthChanging(int value);
    partial void OnCurrentMonthChanged();
    #endregion
		
		public TopSeller()
		{
			this._Product = default(EntityRef<Product>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int ProductID
		{
			get
			{
				return this._ProductID;
			}
			set
			{
				if ((this._ProductID != value))
				{
					if (this._Product.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnProductIDChanging(value);
					this.SendPropertyChanging();
					this._ProductID = value;
					this.SendPropertyChanged("ProductID");
					this.OnProductIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsTopSeller", DbType="Bit NOT NULL")]
		public bool IsTopSeller
		{
			get
			{
				return this._IsTopSeller;
			}
			set
			{
				if ((this._IsTopSeller != value))
				{
					this.OnIsTopSellerChanging(value);
					this.SendPropertyChanging();
					this._IsTopSeller = value;
					this.SendPropertyChanged("IsTopSeller");
					this.OnIsTopSellerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SellCounter", DbType="Int NOT NULL")]
		public int SellCounter
		{
			get
			{
				return this._SellCounter;
			}
			set
			{
				if ((this._SellCounter != value))
				{
					this.OnSellCounterChanging(value);
					this.SendPropertyChanging();
					this._SellCounter = value;
					this.SendPropertyChanged("SellCounter");
					this.OnSellCounterChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CurrentMonth", DbType="Int NOT NULL")]
		public int CurrentMonth
		{
			get
			{
				return this._CurrentMonth;
			}
			set
			{
				if ((this._CurrentMonth != value))
				{
					this.OnCurrentMonthChanging(value);
					this.SendPropertyChanging();
					this._CurrentMonth = value;
					this.SendPropertyChanged("CurrentMonth");
					this.OnCurrentMonthChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Product_TopSeller", Storage="_Product", ThisKey="ProductID", OtherKey="ProductID", IsForeignKey=true)]
		public Product Product
		{
			get
			{
				return this._Product.Entity;
			}
			set
			{
				Product previousValue = this._Product.Entity;
				if (((previousValue != value) 
							|| (this._Product.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Product.Entity = null;
						previousValue.TopSeller = null;
					}
					this._Product.Entity = value;
					if ((value != null))
					{
						value.TopSeller = this;
						this._ProductID = value.ProductID;
					}
					else
					{
						this._ProductID = default(int);
					}
					this.SendPropertyChanged("Product");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
