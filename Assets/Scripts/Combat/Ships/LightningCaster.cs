// Author: Kristián Chovančák
// Created: 28.08.2022
// Copyright: (c) Noxgames
// http://www.noxgames.com/

using System;
using Data;
using UnityEngine;

namespace Combat.Ships
{
    public class LightningCaster<TData> : BaseShip<TData> where TData : BaseShipData
    {
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            throw new NotImplementedException();
        }

        protected override void OnSubCooldown()
        {
            base.OnSubCooldown();

            var target = GameManager.GetClosestAsteroid(transform.position);
            if (target == null) return;
        }
    }
    
    public class LightningCaster : LightningCaster<BaseShipData> { }
}