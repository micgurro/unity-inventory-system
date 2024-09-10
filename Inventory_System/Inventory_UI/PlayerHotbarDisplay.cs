using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace meekobytes
{
    
    public class PlayerHotbarDisplay : StaticInventoryDisplay
    {
        private int _maxIndexSize = 6;
        private int _currentIndex = 0;
        //TODO: Refactor controls to the input manager
        private PlayerControls _playerControls;

        private void Awake()
        {
            _playerControls = new PlayerControls();
        }

        protected override void Start()
        {
            base.Start();

            _currentIndex = 0;
            _maxIndexSize = slots.Length - 1;

            slots[_currentIndex].ToggleHighlight();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _playerControls.Enable();

            _playerControls.PlayerUI.Hotbar1.performed -= Hotbar1;
            _playerControls.PlayerUI.Hotbar2.performed -= Hotbar2;
            _playerControls.PlayerUI.Hotbar3.performed -= Hotbar3;
            _playerControls.PlayerUI.Hotbar4.performed -= Hotbar4;
            _playerControls.PlayerUI.Hotbar5.performed -= Hotbar5;
            _playerControls.PlayerUI.Hotbar6.performed -= Hotbar6;
            _playerControls.PlayerUI.UseItem.performed -= useItem;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _playerControls.Disable();

            _playerControls.PlayerUI.Hotbar1.performed += Hotbar1;
            _playerControls.PlayerUI.Hotbar2.performed += Hotbar2;
            _playerControls.PlayerUI.Hotbar3.performed += Hotbar3;
            _playerControls.PlayerUI.Hotbar4.performed += Hotbar4;
            _playerControls.PlayerUI.Hotbar5.performed += Hotbar5;
            _playerControls.PlayerUI.Hotbar6.performed += Hotbar6;
            _playerControls.PlayerUI.UseItem.performed += useItem;
        }

        private void useItem(InputAction.CallbackContext obj)
        {
            if (slots[_currentIndex].AssignedInventorySlot.ItemData != null) slots[_currentIndex].AssignedInventorySlot.ItemData.UseItem();
        }

        #region Hotbar Controls
        private void Hotbar1(InputAction.CallbackContext obj)
        {
            SetIndex(0);
        }
        private void Hotbar2(InputAction.CallbackContext obj)
        {
            SetIndex(1);
        }
        private void Hotbar3(InputAction.CallbackContext obj)
        {
            SetIndex(2);
        }
        private void Hotbar4(InputAction.CallbackContext obj)
        {
            SetIndex(3);
        }
        private void Hotbar5(InputAction.CallbackContext obj)
        {
            SetIndex(4);
        }
        private void Hotbar6(InputAction.CallbackContext obj)
        {
            SetIndex(5);
        }
        #endregion

        private void Update()
        {
            if (_playerControls.PlayerUI.MouseWheel.ReadValue<float>() > 0.1f) ChangeIndex(1);
            if(_playerControls.PlayerUI.MouseWheel.ReadValue<float>() < -0.1f) ChangeIndex(-1);
        }

        private void UseItem(InputAction.CallbackContext obj)
        {
            if (slots[_currentIndex].AssignedInventorySlot.ItemData != null) slots[_currentIndex].AssignedInventorySlot.ItemData.UseItem();
        }

        private void ChangeIndex(int direction)
        {
            slots[_currentIndex].ToggleHighlight();
            _currentIndex += direction;

            if (_currentIndex > _maxIndexSize) _currentIndex = 0;
            if (_currentIndex < 0) _currentIndex = _maxIndexSize;

            slots[_currentIndex].ToggleHighlight();
        }

        private void SetIndex(int newIndex)
        {
            slots[_currentIndex].ToggleHighlight();
            if (newIndex < 0) _currentIndex = 0;
            if (newIndex > _maxIndexSize) newIndex = _maxIndexSize;

            _currentIndex = newIndex;
            slots[_currentIndex].ToggleHighlight();
        }
    }

}