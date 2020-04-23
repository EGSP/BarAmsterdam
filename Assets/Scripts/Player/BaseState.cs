using UnityEngine;

using Core;
using Interiors;

namespace Player.PlayerStates
{
    /// <summary>
    /// Стартовое поведение персонажа (ходьба, взаимодействие и т.д.)
    /// </summary>
    public class BaseState : PlayerState
    {
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
                
            // Нажатие и удерживание могут совпадать (особенность движка)
            // Нажатие на кнопку
            var horDown = (int)DeviceInput.GetHorizontalAxisDown();
            var verDown = (int)DeviceInput.GetVerticalAxisDown();
            
            // Удерживание кнопки
            var hor = (int)DeviceInput.GetHorizontalAxis();
            var ver = (int)DeviceInput.GetVerticalAxis();

            

            if (DeviceInput.GetHandleButtonDown())
            {
                var pos = Vector3.zero;

                // Добавить ориентацию в игрока и по ней лайнкастить
            }

            // Если движемся или двигались только в одну сторону или только нажали
            if ((Mathf.Abs(hor) + Mathf.Abs(ver)) == 1)
            {
                var pos = Vector3.zero;

                if(hor != 0)
                {
                    // Это означает первое нажатие
                    if(hor == horDown)
                    {
                        //Player.ChangeOrientation(hor, ver);

                        pos = Player.transform.position + Player.transform.right * Player.MoveStep * horDown;
                        // Ищем объект перед нами
                        var interior = Player.GetComponentByLinecast<Interior>(pos);
                        if (interior != null)
                        {
                            #region TableAvoidance
                            //TODO Вообще весь следующий блок должен работать только если interior == Table
                            // Нужно чтобы вызывался метод у наследников Interior.Так как в случае стула у нас pos
                            // будет равный стулу, разные анимации, вобщем поведение зависит от реализации. 

                            // -> Этот обход стола вообще непонятно зачем существует, ошибка геймдизайна игры
                            // -> Георгий сказал, что пока не нужно (22.04.2020)

                            // Если не лицом к столу - разверуться
                            //if (lastHor != hor)
                            //{
                            //    lastHor = hor;
                            //    lastVer = ver;
                            //    return this;
                            //}
                            //Иначе обойти стол
                            //Debug.Log(interior);

                            //var tmp = hor;
                            //hor = ver;
                            //ver = tmp;
                            #endregion
                            return this;
                        }
                    }
                }
                else if(ver != 0)
                {
                    // Это означает первое нажатие
                    if (ver == verDown)
                    {
                        //Player.ChangeOrientation(hor, ver);

                        pos = Player.transform.position + Player.transform.up * Player.MoveStep * verDown;
                        // Ищем объект перед нами
                        var interior = Player.GetComponentByLinecast<Interior>(pos);
                        if (interior != null)
                        {
                            return this;
                        }
                    }
                }
            }

            // Если персонаж должен сдвинутся и он стоит на месте
            if ((hor != 0 || ver != 0) && Player.IsMoving == false)
            {
                Player.Move(hor, ver);
            }

            return this;
        }
    }
}
