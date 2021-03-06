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

namespace Deg.DatabaseManager
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PJMAggregateLMPs2")]
	public partial class PJM5minLMPDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public PJM5minLMPDataContext() : 
				base(global::Deg.DatabaseManager.Properties.Settings.Default.PJMAggregateLMPs2ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PJM5minLMPDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJM5minLMPDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJM5minLMPDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PJM5minLMPDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name= "PJMAggregateLMPs2.dbo.Get5minLMPStartStop")]
		public ISingleResult<Get5minLMPStartStopResult> Get5minLMPStartStop([global::System.Data.Linq.Mapping.ParameterAttribute(Name="StartTime", DbType="DateTime")] System.Nullable<System.DateTime> startTime, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="StopTime", DbType="DateTime")] System.Nullable<System.DateTime> stopTime)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startTime, stopTime);
			return ((ISingleResult<Get5minLMPStartStopResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name= "PJMAggregateLMPs2.dbo.GetMaxTimepoint")]
		public ISingleResult<GetMaxTimepointResult> GetMaxTimepoint()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<GetMaxTimepointResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name= "PJMAggregateLMPs2.dbo.GetLocationName")]
		public ISingleResult<GetLocationNameResult> GetLocationName()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<GetLocationNameResult>)(result.ReturnValue));
		}
	}
	
	public partial class Get5minLMPStartStopResult
	{
		
		private string _LocationName;
		
		private System.Nullable<decimal> _LMP;
		
		private System.Nullable<System.DateTime> _EditTime;
		
		private System.Nullable<System.DateTime> _TimePoint;
		
		public Get5minLMPStartStopResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocationName", DbType="VarChar(50)")]
		public string LocationName
		{
			get
			{
				return this._LocationName;
			}
			set
			{
				if ((this._LocationName != value))
				{
					this._LocationName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LMP", DbType="Decimal(18,2)")]
		public System.Nullable<decimal> LMP
		{
			get
			{
				return this._LMP;
			}
			set
			{
				if ((this._LMP != value))
				{
					this._LMP = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EditTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> EditTime
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TimePoint", DbType="DateTime")]
		public System.Nullable<System.DateTime> TimePoint
		{
			get
			{
				return this._TimePoint;
			}
			set
			{
				if ((this._TimePoint != value))
				{
					this._TimePoint = value;
				}
			}
		}
	}
	
	public partial class GetMaxTimepointResult
	{
		
		private System.Nullable<System.DateTime> _Column1;
		
		public GetMaxTimepointResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="", Storage="_Column1", DbType="DateTime")]
		public System.Nullable<System.DateTime> Column1
		{
			get
			{
				return this._Column1;
			}
			set
			{
				if ((this._Column1 != value))
				{
					this._Column1 = value;
				}
			}
		}
	}
	
	public partial class GetLocationNameResult
	{
		
		private string _location_name;
		
		public GetLocationNameResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_location_name", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string location_name
		{
			get
			{
				return this._location_name;
			}
			set
			{
				if ((this._location_name != value))
				{
					this._location_name = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
