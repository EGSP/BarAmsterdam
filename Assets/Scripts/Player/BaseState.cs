using UnityEngine;
using System;

using Interiors;
using Items.MonoItems;

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

        public PlayerState Pose(UpdateData updateData)
        {
            var pos = Vector3.zero;
            pos = Player.transform.position + Player.ModifiedOrientation;
                
            // Ищем объект перед нами
            var interior = Player.GetComponentByLinecast<Interior>(pos);
            if (interior != null)
            {
                return interior.GetPlayerState(Player);
            }

            return this;
        }

        public override PlayerState Move(UpdateData updateData)
        {
            // Удерживание кнопки
            int hor = updateData.HorizontalAxisInput;
            int ver = updateData.VerticalAxisInput;

            var cursor = Player.TableCursor;

            // Если есть удержание по одной из осей
            if (hor != 0 || ver != 0)
            {
                cursor.Cancel();

                // Если персонаж должен сдвинутся и он стоит на месте
                if ((hor != 0 || ver != 0) && Player.IsMoving == false)
                {
                    Player.Move(hor, ver);

                    if (hor > 0)
                    {
                        Player.SpriteRenderer.flipX = false;
                        Player.Animator.Play("MoveRight");
                    }
                        
                    else if (hor < 0)
                    {
                        Player.SpriteRenderer.flipX = true;
                        Player.Animator.Play("MoveRight");
                    }
                    else if (ver > 0)
                    {
                        Player.Animator.Play("MoveUp");
                    }
                    else if (ver < 0)
                    {
                        Player.Animator.Play("MoveDown");
                    }
                }
                
                return this;

            }

            // Нажатие на кнопку
            int horDown = updateData.HorizontalAxisDownInput;
            int verDown = updateData.VerticalAxisDownInput;

            // Если нажали на одну из кнопок
            if(horDown != 0 || verDown != 0)
            {
                cursor.Cancel();

                Player.ChangeOrientation(horDown, verDown);

                // Принимаем позу
                return Pose(updateData);
            }

            return this;
        }

        public override PlayerState Handle(UpdateData updateData)
        {
            var cursor = Player.TableCursor;
            var tableTop = Player.GetComponentByLinecast<TableTop>(
                Player.transform.position + Player.ModifiedOrientation);

            Debug.Log(cursor.IsActive);
            MonoItem item;
            if (cursor.IsActive)
            {
                item = tableTop.TakeItemByReference(cursor.GetSelectedItem()) as MonoItem;
            }
            else
            {
                item = tableTop.TakeItemByDistance(Player.transform.position) as MonoItem;
            }
            
            item.transform.parent = Player.transform;
            return item.GetPlayerState(Player);
        }

        public override PlayerState Action(UpdateData updateData)
        {
            Debug.Log("Smoking");
            
            return this;
        }

        public override PlayerState Extra(UpdateData updateData)
        {
            var cursor = Player.TableCursor;
            
            // Если курсор ни на что не указывает
            if(cursor.IsActive == false)
            {
                var cursorEnumerable = Player.GetComponentByLinecast<ICursorEnumerable>
                    (Player.transform.position + Player.ModifiedOrientation);

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
    }
}
