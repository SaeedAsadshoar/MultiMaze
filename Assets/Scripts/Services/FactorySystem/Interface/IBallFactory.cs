﻿using System.Threading.Tasks;
using Domain.Enum;
using Domain.Interface;

namespace Services.FactorySystem.Interface
{
    public interface IBallFactory
    {
        IFactoryObject GetBall(BallTypes ballType);
        Task LoadAllBalls();
    }
}