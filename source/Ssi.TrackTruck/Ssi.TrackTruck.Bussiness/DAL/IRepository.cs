﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ssi.TrackTruck.Bussiness.DAL.Entities;

namespace Ssi.TrackTruck.Bussiness.DAL
{
    public interface IRepository
    {
        T FindOne<T>(Expression<Func<T, bool>> condition);
        T Create<T>(T user);
        IQueryable<T> GetAll<T>();
        IQueryable<T> WhereIn<T, TProp>(Expression<Func<T, TProp>> property, IEnumerable<TProp> values);
    }
}
