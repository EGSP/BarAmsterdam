using UnityEngine;
using System;
using System.Collections.Generic;
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

        public override PlayerState Awake()
        {
            return this;
        }

        public PlayerState Pose(UpdateData updateData)
        {
            var pos = Vector3.zero;
            pos = Player.transform.position + Player.ModifiedOrientation;
                
            // Ищем объект перед нами
            var interior = Player.GetComponentByLinecast<Interior>(pos);
            if (interior != null)
            {
                return interior.GetPlayerState(Player).Awake();
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
                
                Player.ChangeOrientation(hor, ver);

                // Если персонаж должен сдвинутся и он стоит на месте
                if ((hor != 0 || ver != 0) && Player.IsMoving == false)
                {
                    Player.Move(hor, ver);

                    if (hor > 0)
                    {
                        Player.SpriteRenderer.flipX = false;
                        Player.PlayAnimation("MoveRight");
                    }
                        
                    else if (hor < 0)
                    {
                        Player.SpriteRenderer.flipX = true;
                        Player.PlayAnimation("MoveRight");
                    }
                    else if (ver > 0)
                    {
                        Player.PlayAnimation("MoveUp");
                    }
                    else if (ver < 0)
                    {
                        Player.PlayAnimation("MoveDown");
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
            var tableTop = FindAcceptableObject<TableTop>();

            // Если стола рядом нет
            if (tableTop == null)
            {
                NoTableTopWarning();
                return this;
            }

            Debug.Log($"Cursor active: {cursor.IsActive}");
            MonoItem item;
            if (cursor.IsActive)
            {
                item = tableTop.TakeItemByReference(cursor.GetSelectedItem()) as MonoItem;
            }
            else
            {
                item = tableTop.TakeItemByDistance(Player.transform.position) as MonoItem;
            }
            
            cursor.Cancel();

            if (item != null)
            {
                Player.PlaceItemToHand(item);
                return item.GetPlayerState(Player).Awake();
            }

            return this;
        }

        public override PlayerState Action(UpdateData updateData)
        {
            Player.TableCursor.Cancel();
            Debug.Log("Smoking");
            
            return this;
        }

        public override PlayerState Extra(UpdateData updateData)
        {
            var cursor = Player.TableCursor;
            
            // Если курсор ни на что не указывает
            if(cursor.IsActive == false)
            {
                var cursorEnumerable = FindAcceptableObject<ICursorEnumerable>();

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

        /// <summary>
        /// Получение подходящего объекта рядом
        /// </summary>
        /// <returns></returns>
        public T FindAcceptableObject<T>() where T: class
        {
            // Направление взгляда, сверху и снизу
            var pointList = new List<Vector3>
            {
                Player.transform.position + Player.ModifiedOrientation,
                Player.transform.position+ Player.GetModifiedDirection(Player.Orientation.LocalLeft),
                Player.transform.position + Player.GetModifiedDirection(Player.Orientation.LocalRight)
            };

            for (var i = 0; i < pointList.Count; i++)
            {
                var tObject = Player.GetComponentByLinecast<T>(
                    pointList[i]);

                if (tObject != null)
                    return tObject;
            }

            return null;
        }
        
        /// <summary>
        /// Сообщить об отсутствии стола
        /// </summary>
        public void NoTableTopWarning()
        {
            Debug.Log("По направлению взгляда нет стола");
        }

        public void NoIteractItemWarning(Type type)
        {
            Debug.Log($"Не найден предмет для взаимодействия: {type.ToString()}");
        }
    }
}
