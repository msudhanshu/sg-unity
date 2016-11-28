using System;
using System.Collections.Generic;
using System.Linq;

namespace KiwiCommonDatabase
{
	/*
 	* The db query attributes, that are applied on the querybuilder in ormlite
 	* For eg. limit - the max no. of item to be returned by the query; selectedColumns - the columns, which are returned in the db query
 	*/
	public class KDbQueryAttribute {
		public static long DEFAULT_QUERY_LIMIT = -1;
		public static long DEFAULT_QUERY_OFFSET = -1;
		
		protected long limit = DEFAULT_QUERY_LIMIT;
		protected List<string> selectedColumns = null;
		protected List<OrderByValue> orderByColumnList = null;
		protected long offset = DEFAULT_QUERY_OFFSET;

		public KDbQueryAttribute(){
		}

		public KDbQueryAttribute(long limit_){
			limit = limit_;
		}
		
		public KDbQueryAttribute(string column){
			selectedColumns = new List<string>();
			selectedColumns.Add(column);
		}
		
		public KDbQueryAttribute(params string[] columns){
			if(columns != null && columns.Length > 0){
				selectedColumns = columns.OfType<string>().ToList();
			}
		}
		
		public KDbQueryAttribute(long limit_, string column) :this(limit_){
			selectedColumns = new List<string>();
			selectedColumns.Add(column);
		}
		
		public KDbQueryAttribute(long limit_, List<string> columns) :this(limit_){
			if(columns != null && columns.Count > 0){
				selectedColumns = new List<string>();
				selectedColumns.AddRange(columns);
			}
		}

		public KDbQueryAttribute(string orderByColumn_, bool isAscending_){
			orderByColumnList = new List<OrderByValue>();
			orderByColumnList.Add(new OrderByValue(orderByColumn_, isAscending_));
		}
		
		public KDbQueryAttribute(long limit_, string orderByColumn_, bool isAscending_) :this(limit_){
			orderByColumnList = new List<OrderByValue>();
			orderByColumnList.Add(new OrderByValue(orderByColumn_, isAscending_));
		}

		public KDbQueryAttribute(List<OrderByValue> orderByColumnList_){
			orderByColumnList = orderByColumnList_;
		}

		public KDbQueryAttribute(long limit_, List<OrderByValue> orderByColumnList_) :this(limit_){
			orderByColumnList = orderByColumnList_;
		}

		// Limit
		public void setLimit(long newLimit){
			limit = newLimit;
		}

		public long getLimit(){
			return limit;
		}
		
		public bool hasNonDefaultLimit(){
			return limit != DEFAULT_QUERY_LIMIT;
		}

		// Offset
		public bool hasNonDefaultOffset(){
			return offset != DEFAULT_QUERY_OFFSET;
		}
		
		public long getOffset(){
			return offset;
		}
		
		public void setOffset(long offset_){
			this.offset = offset_;
		}

		// Select columns
		public bool hasSelectedColumns(){
			if(selectedColumns != null && selectedColumns.Count > 0){
				return true;	
			}
			return false;
		}
		
		public List<string> getSelectedColumnsList(){
			return selectedColumns;
		}
		
		public string[] getSelectedColumnsArray(){
			if(selectedColumns != null && selectedColumns.Count > 0){
				return selectedColumns.ToArray();	//new string[selectedColumns.size()]);	
			}
			return null;
		}

		public void addSelectedColumn(string columnName){
			if(selectedColumns == null && columnName != null && columnName.Length > 0){
				selectedColumns = new List<string>();
			}
			selectedColumns.Add (columnName);
		}

		// Order by
		public bool hasOrderByColumns(){
			if(orderByColumnList != null && orderByColumnList.Count > 0){
				return true;	
			}
			return false;
		}
		
		public List<OrderByValue> getOrderByColumnsList(){
			return orderByColumnList;
		}
		
		public List<OrderByValue> addOrderByColumn(string column, bool ascending){
			if(orderByColumnList == null){
				orderByColumnList = new List<OrderByValue>();
			}
			orderByColumnList.Add(new OrderByValue(column, ascending));
			return orderByColumnList;
		}
		
		public List<OrderByValue> addOrderByColumns(List<OrderByValue> columnsList){
			if(orderByColumnList == null){
				orderByColumnList = new List<OrderByValue>();
			}
			orderByColumnList.AddRange(columnsList);
			return orderByColumnList;
		}
		
	}

}

