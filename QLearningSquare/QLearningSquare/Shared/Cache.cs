﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.Shared
{
    

    public class CacheItem
    {
        public object Store;
        public string Key;
    }


    public enum CacheReplacementPolicy
    {
        FIFO,
    }

    class Cache
    {
        private class CacheItemInfo : IComparable<CacheItemInfo>
        {
            internal TimeSpan Time;
            internal CacheItem Data;
            internal CacheReplacementPolicy replacementPolicy;

            public int CompareTo(CacheItemInfo other)
            {
                if (replacementPolicy == other.replacementPolicy  && replacementPolicy == CacheReplacementPolicy.FIFO)
                {
                    if (Time > other.Time)
                        return 1;
                    else if (Time < other.Time)
                        return -1;
                    else
                        return 0;
                    
                }

                throw new Exception("Not allowed comparision");
            }
        }

        int hitCount;
        int totalCount;
        CacheReplacementPolicy replacementPolicy;
        double hitrate;

        Dictionary<string, CacheItemInfo> values;
        int length;

        public Cache(int length,CacheReplacementPolicy replacementPolicy = CacheReplacementPolicy.FIFO)
        {
            values = new Dictionary<string, CacheItemInfo>();
            this.length = length;
        }
        
        public void Add(CacheItem item)
        {
            CacheItemInfo i = new CacheItemInfo();
            i.Time = new TimeSpan();
            i.Data = item;
            
            if(values.Count < length)
                values[item.Key] = i;

            else
            {
                substitute(i);
            }

            totalCount++;
        }

        public CacheItem Get(string itemKey)
        {
            totalCount++;
            if (values.ContainsKey(itemKey))
            {
                hitCount++;
                return values[itemKey].Data;
            }

            return null;
        }

        private void substitute( CacheItemInfo newItem)
        {
            CacheItemInfo toRemove = (Enumerable.ToList(values.Values)).Max();
            values.Remove(toRemove.Data.Key);

            values.Add(newItem.Data.Key, newItem);
        }

        public double Hitrate { get => totalCount > 0 ? hitCount/totalCount : 0;}
        internal CacheReplacementPolicy ReplacementPolicy1 { get => replacementPolicy; set => replacementPolicy = value; }
    }
}
