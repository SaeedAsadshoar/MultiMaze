using UnityEngine;

namespace Services.Toast.Interface
{
    public interface IToastService
    {
        void ShowToast(string title, string message, Sprite thumb);
    }
}