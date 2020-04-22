using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.PlayerStates
{
    public abstract class PlayerState : IDisposable
    {
        /// <summary>
        /// Игрок этого состояния
        /// </summary>
        public readonly PlayerController Player;
                                                                                                                                                                                                                                                 
        public PlayerState(PlayerController player)
        {
            if (player == null)
                throw new System.NullReferenceException();

            Player = player;
        }

        /// <summary>
        /// Обновление состояния 
        /// </summary>
        /// <param name="updateData">Данные для обновления</param>
        public abstract PlayerState UpdateState(UpdateData updateData);
        
        /// <summary>
        /// Высвобождение ресурсов
        /// </summary>
        public abstract void Dispose();

    }

    public class UpdateData
    {
        public float deltaTime;
    }

    /// <summary>
    /// Стартовое поведение персонажа (ходьба, взаимодействие и т.д.)
    /// </summary>
    public class BaseState : PlayerState
    {
        //Указывает куда смотрит игрок
        public int lastVer = 0;
        public int lastHor = 0;
        
        
        public string itemInHand = "Nothing";
        
        public BaseState(PlayerController player) : base(player)
        {

        }

        // Нечего высввобождать
        public override void Dispose()
        {
            return;
        }

        public override PlayerState UpdateState(UpdateData updateData)
        {
            Debug.Log(itemInHand);
            
            if (DeviceInput.isZBtnDown())
            {
                var pos = Vector3.zero;
                if (lastHor != 0)
                {
                    pos = Player.transform.position + Player.transform.right * Player.MoveStep * lastHor;
                    // Ищем объект перед нами
                    var interior = Player.GetComponentByLinecast<Interior>(pos);
                    if (itemInHand == "Nothing")
                        itemInHand = Player.TakeItem(interior);
                    else
                        itemInHand = Player.PutItem(interior);
                }
                else if (lastVer != 0)
                {
                    pos = Player.transform.position + Player.transform.up * Player.MoveStep * lastVer;
                    // Ищем объект перед нами
                    var interior = Player.GetComponentByLinecast<Interior>(pos);
                    if (itemInHand == "Nothing")
                        itemInHand = Player.TakeItem(interior);
                    else
                        itemInHand = Player.PutItem(interior);
                }
                
            }
                
            // Нажатие и удерживание могут совпадать (особенность движка)
            // Нажатие на кнопку
            var horDown = (int)DeviceInput.GetHorizontalAxisDown();
            var verDown = (int)DeviceInput.GetVerticalAxisDown();
            
            // Удерживание кнопки
            var hor = (int)DeviceInput.GetHorizontalAxis();
            var ver = (int)DeviceInput.GetVerticalAxis();
            

            // Если движемся или двигались только в одну сторону или только нажали
            if ((Mathf.Abs(hor) + Mathf.Abs(ver)) == 1)
            {
                var pos = Vector3.zero;

                if(hor != 0)
                {
                    // Это означает первое нажатие
                    if(hor == horDown)
                    {
                        pos =Player.transform.position + Player.transform.right * Player.MoveStep * horDown;
                        // Ищем объект перед нами
                        var interior = Player.GetComponentByLinecast<Interior>(pos);
                        if (interior != null)
                        {
                            // TODO Вообще весь следующий блок должен работать только если interior == Table
                            // Нужно чтобы вызывался метод у наследников Interior. Так как в случае стула у нас pos
                            // будет равный стулу, разные анимации, вобщем поведение зависит от реализации. 
                            
                            // Если не лицом к столу - разверуться
                            if (lastHor != hor)
                            {
                                lastHor = hor;
                                lastVer = ver;
                                return this;
                            }
                            // Иначе обойти стол
                            Debug.Log(interior);
                            
                            var tmp = hor;
                            hor = ver;
                            ver = tmp;

                        }
                    }
                }
                else if(ver != 0)
                {
                    // Это означает первое нажатие
                    if (ver == verDown)
                    {
                        pos = Player.transform.position + Player.transform.up * Player.MoveStep * verDown;
                        // Ищем объект перед нами
                        var interior = Player.GetComponentByLinecast<Interior>(pos);
                        if (interior != null)
                        {
                            // Если не лицом к столу - разверуться
                            if (lastVer != ver)
                            {
                                lastHor = hor;
                                lastVer = ver;
                                return this;
                            }
                            // Иначе обойти стол
                            Debug.Log(interior);
                            var tmp = hor;
                            hor = ver;
                            ver = tmp;

                        }
                    }
                }
            }

            // Если персонаж должен сдвинутся и он стоит на месте
            if ((hor != 0 || ver != 0) && Player.IsMoving == false)
            {
                lastHor = hor;
                lastVer = ver;
                
                Player.Move(hor, ver);
            }
                

            return this;
        }
    }
}
