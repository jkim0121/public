﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Deg.DatabaseManager.DataDefinitions
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PJMInstantaneousDispatchRates")]
	public partial class PJMDispRatesWebPageDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public PJMDispRatesWebPageDataContext() : 
				base(global::Deg.DatabaseManager.Properties.Settings.Default.PJMInstantaneousDispatchRatesConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PJMDispRatesWebPageDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJMDispRatesWebPageDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJMDispRatesWebPageDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJMDispRatesWebPageDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Data> Datas
		{
			get
			{
				return this.GetTable<Data>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Data")]
	public partial class Data
	{
		
		private System.DateTime _Timepoint;
		
		private System.DateTime _EditTime;
		
		private System.Nullable<float> _DataZone1;
		
		private System.Nullable<float> _DataZone2;
		
		private System.Nullable<float> _DataZone3;
		
		private System.Nullable<float> _DataZone4;
		
		private System.Nullable<float> _DataZone5;
		
		private System.Nullable<float> _DataZone6;
		
		private System.Nullable<float> _DataZone7;
		
		private System.Nullable<float> _DataZone8;
		
		private System.Nullable<float> _DataZone9;
		
		private System.Nullable<float> _DataZone10;
		
		private System.Nullable<float> _DataZone11;
		
		private System.Nullable<float> _DataZone12;
		
		private System.Nullable<float> _DataZone13;
		
		private System.Nullable<float> _DataZone14;
		
		private System.Nullable<float> _DataZone15;
		
		private System.Nullable<float> _DataZone16;
		
		private System.Nullable<float> _DataZone17;
		
		private System.Nullable<float> _DataZone18;
		
		private System.Nullable<float> _DataZone19;
		
		public Data()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Timepoint", DbType="DateTime NOT NULL")]
		public System.DateTime Timepoint
		{
			get
			{
				return this._Timepoint;
			}
			set
			{
				if ((this._Timepoint != value))
				{
					this._Timepoint = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EditTime", DbType="DateTime NOT NULL")]
		public System.DateTime EditTime
		{
			get
			{
				return this._EditTime;
			}
			set
			{
				if ((this._EditTime != value))
				{
					this._EditTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone1", DbType="Real")]
		public System.Nullable<float> DataZone1
		{
			get
			{
				return this._DataZone1;
			}
			set
			{
				if ((this._DataZone1 != value))
				{
					this._DataZone1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone2", DbType="Real")]
		public System.Nullable<float> DataZone2
		{
			get
			{
				return this._DataZone2;
			}
			set
			{
				if ((this._DataZone2 != value))
				{
					this._DataZone2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone3", DbType="Real")]
		public System.Nullable<float> DataZone3
		{
			get
			{
				return this._DataZone3;
			}
			set
			{
				if ((this._DataZone3 != value))
				{
					this._DataZone3 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone4", DbType="Real")]
		public System.Nullable<float> DataZone4
		{
			get
			{
				return this._DataZone4;
			}
			set
			{
				if ((this._DataZone4 != value))
				{
					this._DataZone4 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone5", DbType="Real")]
		public System.Nullable<float> DataZone5
		{
			get
			{
				return this._DataZone5;
			}
			set
			{
				if ((this._DataZone5 != value))
				{
					this._DataZone5 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone6", DbType="Real")]
		public System.Nullable<float> DataZone6
		{
			get
			{
				return this._DataZone6;
			}
			set
			{
				if ((this._DataZone6 != value))
				{
					this._DataZone6 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone7", DbType="Real")]
		public System.Nullable<float> DataZone7
		{
			get
			{
				return this._DataZone7;
			}
			set
			{
				if ((this._DataZone7 != value))
				{
					this._DataZone7 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone8", DbType="Real")]
		public System.Nullable<float> DataZone8
		{
			get
			{
				return this._DataZone8;
			}
			set
			{
				if ((this._DataZone8 != value))
				{
					this._DataZone8 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone9", DbType="Real")]
		public System.Nullable<float> DataZone9
		{
			get
			{
				return this._DataZone9;
			}
			set
			{
				if ((this._DataZone9 != value))
				{
					this._DataZone9 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone10", DbType="Real")]
		public System.Nullable<float> DataZone10
		{
			get
			{
				return this._DataZone10;
			}
			set
			{
				if ((this._DataZone10 != value))
				{
					this._DataZone10 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone11", DbType="Real")]
		public System.Nullable<float> DataZone11
		{
			get
			{
				return this._DataZone11;
			}
			set
			{
				if ((this._DataZone11 != value))
				{
					this._DataZone11 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone12", DbType="Real")]
		public System.Nullable<float> DataZone12
		{
			get
			{
				return this._DataZone12;
			}
			set
			{
				if ((this._DataZone12 != value))
				{
					this._DataZone12 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone13", DbType="Real")]
		public System.Nullable<float> DataZone13
		{
			get
			{
				return this._DataZone13;
			}
			set
			{
				if ((this._DataZone13 != value))
				{
					this._DataZone13 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone14", DbType="Real")]
		public System.Nullable<float> DataZone14
		{
			get
			{
				return this._DataZone14;
			}
			set
			{
				if ((this._DataZone14 != value))
				{
					this._DataZone14 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone15", DbType="Real")]
		public System.Nullable<float> DataZone15
		{
			get
			{
				return this._DataZone15;
			}
			set
			{
				if ((this._DataZone15 != value))
				{
					this._DataZone15 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone16", DbType="Real")]
		public System.Nullable<float> DataZone16
		{
			get
			{
				return this._DataZone16;
			}
			set
			{
				if ((this._DataZone16 != value))
				{
					this._DataZone16 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone17", DbType="Real")]
		public System.Nullable<float> DataZone17
		{
			get
			{
				return this._DataZone17;
			}
			set
			{
				if ((this._DataZone17 != value))
				{
					this._DataZone17 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone18", DbType="Real")]
		public System.Nullable<float> DataZone18
		{
			get
			{
				return this._DataZone18;
			}
			set
			{
				if ((this._DataZone18 != value))
				{
					this._DataZone18 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataZone19", DbType="Real")]
		public System.Nullable<float> DataZone19
		{
			get
			{
				return this._DataZone19;
			}
			set
			{
				if ((this._DataZone19 != value))
				{
					this._DataZone19 = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
