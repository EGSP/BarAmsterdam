using UnityEngine;

using Core;
using Interiors;

using Player.Controllers;
using Player.PlayerCursors;

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
                
            // Удерживание кнопки
            var hor = (int)DeviceInput.GetHorizontalAxis();
            var ver = (int)DeviceInput.GetVerticalAxis();

            var cursor = Player.TableCursor;


            // Если есть удержание по одной из осей
            if (hor != 0 || ver != 0)
            {
                cursor.Cancel();

                // Если персонаж должен сдвинутся и он стоит на месте
                if ((hor != 0 || ver != 0) && Player.IsMoving == false)
                {
                    Player.Move(hor, ver);
                }
                
                return this;

            }

            // Нажатие на кнопку
            var horDown = (int)DeviceInput.GetHorizontalAxisDown();
            var verDown = (int)DeviceInput.GetVerticalAxisDown();

            // Если нажали на одну из кнопок
            if(horDown != 0 || verDown != 0)
            {
                cursor.Cancel();

                var pos = Vector3.zero;

                // Учитывается одновременное нажатие
                if (horDown != 0)
                {
                    pos = Player.transform.position + Player.transform.right * Player.MoveStep * horDown;

                }else if(verDown != 0)
                {
                    pos = Player.transform.position + Player.transform.up * Player.MoveStep * verDown;
                }

                Player.ChangeOrientation(horDown, verDown);

                // Ищем объект перед нами
                var interior = Player.GetComponentByLinecast<Interior>(pos);
                if (interior != null)
                {
                    
                }
                
                return this;
            }

            //
            // Если не нажали на кнопку передвижения
            //

            // Нажатие на кнопку Z
            if (DeviceInput.GetHandleButtonDown())
            {

            }

            // Нажатие на Space
            if (DeviceInput.GetExtraButtonDown())
            {
                // Если курсор ни на что не указывает
                if(cursor.IsActive == false)
                {
                    var cursorEnumerable = Player.GetComponentByLinecast<ICursorEnumerable>(Player.transform.position + Player.ModifiedOrientation);

                    if(cursorEnumerable != null)
                    {
                        var cursorEnumerator = new CursorEnumerator(cursorEnumerable);
                        cursor.SetCursorEnumerator(cursorEnumerator);
                    }
                    else
                    {
                        cursor.Cancel();
                    }
                }
                else
                {
                    cursor.Next();
                }

                return this;
            }

            return this;
        }
    }
}
