using GraphicsTemplate.Models;
using System;

namespace GraphicsTemplate.Shared
{
    public interface IGraphicsService
    {
        void SetTransform(Guid id, Transform t);
    }
}
