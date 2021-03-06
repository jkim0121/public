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
	public partial class PJMDispRatesEdataDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public PJMDispRatesEdataDataContext() : 
				base(global::Deg.DatabaseManager.Properties.Settings.Default.PJMInstantaneousDispatchRatesConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PJMDispRatesEdataDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJMDispRatesEdataDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJMDispRatesEdataDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJMDispRatesEdataDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Data_eDataFeed> Data_eDataFeed
		{
			get
			{
				return this.GetTable<Data_eDataFeed>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name= "[PJMInstantaneousDispatchRates].[dbo].[Data_eDataFeed]")]
	public partial class Data_eDataFeed
	{
		
		private System.DateTime _Timepoint;
		
		private System.DateTime _EditTime;
		
		private System.Nullable<float> _AE;
		
		private System.Nullable<float> _AEP;
		
		private System.Nullable<float> _ATSI;
		
		private System.Nullable<float> _BC;
		
		private System.Nullable<float> _COMED;
		
		private System.Nullable<float> _DAY;
		
		private System.Nullable<float> _DEOK;
		
		private System.Nullable<float> _DOM;
		
		private System.Nullable<float> _DPL;
		
		private System.Nullable<float> _DUQ;
		
		private System.Nullable<float> _JC;
		
		private System.Nullable<float> _METED;
		
		private System.Nullable<float> _PE;
		
		private System.Nullable<float> _PENELEC;
		
		private System.Nullable<float> _PEP;
		
		private System.Nullable<float> _PJM_WEST;
		
		private System.Nullable<float> _PL;
		
		private System.Nullable<float> _PS;
		
		private System.Nullable<float> _AP;
		
		private System.Nullable<float> _EKPC;
		
		public Data_eDataFeed()
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AE", DbType="Real", Name ="AE")]
		public System.Nullable<float> AE
		{
			get
			{
				return this._AE;
			}
			set
			{
				if ((this._AE != value))
				{
					this._AE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AEP", DbType="Real", Name ="AEP")]
		public System.Nullable<float> AEP
		{
			get
			{
				return this._AEP;
			}
			set
			{
				if ((this._AEP != value))
				{
					this._AEP = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ATSI", DbType="Real", Name ="ATSI")]
		public System.Nullable<float> ATSI
		{
			get
			{
				return this._ATSI;
			}
			set
			{
				if ((this._ATSI != value))
				{
					this._ATSI = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BC", DbType="Real", Name ="BC")]
		public System.Nullable<float> BC
		{
			get
			{
				return this._BC;
			}
			set
			{
				if ((this._BC != value))
				{
					this._BC = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_COMED", DbType="Real", Name ="COMED")]
		public System.Nullable<float> COMED
		{
			get
			{
				return this._COMED;
			}
			set
			{
				if ((this._COMED != value))
				{
					this._COMED = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DAY", DbType="Real", Name ="DAY")]

		public System.Nullable<float> DAY
		{
			get
			{
				return this._DAY;
			}
			set
			{
				if ((this._DAY != value))
				{
					this._DAY = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DEOK", DbType="Real", Name ="DEOK")]
		public System.Nullable<float> DEOK
		{
			get
			{
				return this._DEOK;
			}
			set
			{
				if ((this._DEOK != value))
				{
					this._DEOK = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DOM", DbType="Real", Name ="DOM")]
		public System.Nullable<float> DOM
		{
			get
			{
				return this._DOM;
			}
			set
			{
				if ((this._DOM != value))
				{
					this._DOM = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DPL", DbType="Real",Name = "DPL")]
		public System.Nullable<float> DPL
		{
			get
			{
				return this._DPL;
			}
			set
			{
				if ((this._DPL != value))
				{
					this._DPL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DUQ", DbType="Real", Name ="DUQ")]
		public System.Nullable<float> DUQ
		{
			get
			{
				return this._DUQ;
			}
			set
			{
				if ((this._DUQ != value))
				{
					this._DUQ = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_JC", DbType="Real", Name ="JC")]
		public System.Nullable<float> JC
		{
			get
			{
				return this._JC;
			}
			set
			{
				if ((this._JC != value))
				{
					this._JC = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_METED", DbType="Real", Name="METED")]
		public System.Nullable<float> METED
		{
			get
			{
				return this._METED;
			}
			set
			{
				if ((this._METED != value))
				{
					this._METED = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PE", DbType="Real", Name ="PE")]
		public System.Nullable<float> PE
		{
			get
			{
				return this._PE;
			}
			set
			{
				if ((this._PE != value))
				{
					this._PE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PENELEC", DbType="Real", Name ="PE")]
		public System.Nullable<float> PENELEC
		{
			get
			{
				return this._PENELEC;
			}
			set
			{
				if ((this._PENELEC != value))
				{
					this._PENELEC = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PEP", DbType="Real", Name ="PEP")]
		public System.Nullable<float> PEP
		{
			get
			{
				return this._PEP;
			}
			set
			{
				if ((this._PEP != value))
				{
					this._PEP = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[PJM WEST]", Storage="_PJM_WEST", DbType="Real")]
		public System.Nullable<float> PJM_WEST
		{
			get
			{
				return this._PJM_WEST;
			}
			set
			{
				if ((this._PJM_WEST != value))
				{
					this._PJM_WEST = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PL", DbType="Real", Name="PL")]
		public System.Nullable<float> PL
		{
			get
			{
				return this._PL;
			}
			set
			{
				if ((this._PL != value))
				{
					this._PL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PS", DbType="Real", Name ="PS")]
		public System.Nullable<float> PS
		{
			get
			{
				return this._PS;
			}
			set
			{
				if ((this._PS != value))
				{
					this._PS = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AP", DbType="Real", Name ="AP")]
		public System.Nullable<float> AP
		{
			get
			{
				return this._AP;
			}
			set
			{
				if ((this._AP != value))
				{
					this._AP = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EKPC", DbType="Real", Name ="EKPC")]
		public System.Nullable<float> EKPC
		{
			get
			{
				return this._EKPC;
			}
			set
			{
				if ((this._EKPC != value))
				{
					this._EKPC = value;
				}
			}
		}



        ////public ICollection <float?> PriceObjects
        ////{
        ////    get
        ////    {
        ////        return new float?[] { AE, AEP, ATSI, BC, COMED, DAY, DEOK, DOM, DPL, DUQ, JC, METED, PE, PENELEC, PEP, PJM_WEST, PL, PS, AP, EKPC };
        ////    }
        ////}


	}
}
#pragma warning restore 1591
