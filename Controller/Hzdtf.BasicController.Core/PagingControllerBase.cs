﻿using Hzdtf.Service.Contract.Standard;
using Hzdtf.Utility.Standard.Attr;
using Hzdtf.Utility.Standard.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Hzdtf.Utility.Standard.Model.Return;
using Hzdtf.Utility.Standard.Model.Page;

namespace Hzdtf.BasicController.Core
{
    /// <summary>
    /// 分页控制器基类
    /// @ 黄振东
    /// </summary>
    /// <typeparam name="PageInfoT">页面信息类型</typeparam>
    /// <typeparam name="ModelT">模型类型</typeparam>
    /// <typeparam name="ServiceT">服务类型</typeparam>
    /// <typeparam name="PageFilterT">分页筛选类型</typeparam>
    public abstract class PagingControllerBase<PageInfoT, ModelT, ServiceT, PageFilterT> : PageControllerBase<PageInfoT>
        where PageInfoT : PageInfo
        where ModelT : SimpleInfo
        where ServiceT : IService<ModelT>
        where PageFilterT : FilterInfo
    {
        /// <summary>
        /// 服务
        /// </summary>
        public ServiceT Service
        {
            get;
            set;
        }

        /// <summary>
        /// 执行分页获取数据
        /// </summary>
        /// <returns>分页返回信息</returns>
        [HttpGet]
        [Function(FunCodeDefine.QUERY_CODE)]
        public virtual object Page()
        {
            ReturnInfo<PagingInfo<ModelT>> returnInfo = DoPage();
            return PagingReturnConvert.Convert<ModelT>(returnInfo);
        }

        /// <summary>
        /// 去分页
        /// </summary>
        /// <returns>返回信息</returns>
        protected virtual ReturnInfo<PagingInfo<ModelT>> DoPage()
        {
            int pageIndex, pageSize;
            PageFilterT filter = PagingParseFilter.ToFilterObjectFromHttp<PageFilterT>(Request, out pageIndex, out pageSize);
            AppendFilterParams(filter);

            ReturnInfo<PagingInfo<ModelT>> returnInfo = QueryPageFromService(pageIndex, pageSize, filter);
            AfterPage(returnInfo, pageIndex, pageSize, filter);

            return returnInfo;
        }

        /// <summary>
        /// 从服务里查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="filter">筛选</param>
        /// <returns>返回信息</returns>
        protected virtual ReturnInfo<PagingInfo<ModelT>> QueryPageFromService(int pageIndex, int pageSize, PageFilterT filter) => Service.QueryPage(pageIndex, pageSize, filter);

        /// <summary>
        /// 分页后
        /// </summary>
        /// <param name="returnInfo">返回信息</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="filter">筛选</param>
        /// <returns>返回信息</returns>
        protected virtual void AfterPage(ReturnInfo<PagingInfo<ModelT>> returnInfo, int pageIndex, int pageSize, PageFilterT filter) { }

        /// <summary>
        /// 追加筛选参数
        /// </summary>
        /// <param name="pageFilter">分页筛选</param>
        protected virtual void AppendFilterParams(PageFilterT pageFilter) { }
    }
}
