using HarmonyLib;
using System;
using System.Xml.Linq;

namespace OpenFun_Claim_Loot_Protect_Mod
{
	public class GameManager_OpenTileEntityAllowed
	{
		[HarmonyPatch(typeof(GameManager), "OpenTileEntityAllowed")]
		public class OnOpenTileEntityAllowed_Patch
		{
			private static bool Prefix(ref bool __result, int _entityIdThatOpenedIt, TileEntity _te)
            {
				if(_te == null) 
				{
                    return true;
                }
				ClientInfo _cInfo = GetClient(_entityIdThatOpenedIt);
				if (_cInfo == null)
				{
					return true;
				}
				string name;
				if (_te.EntityId != -1)
				{
                    name = GameManager.Instance.World.GetEntity(_te.EntityId).name;
                }
				else
				{
                    name = _te.blockValue.Block.GetBlockName();
                }
                try
				{
					if (Settings.DeniedTileEntity.Contains(name) && !CanOpenLootContainer(_cInfo, _te))
					{
						__result = false;
						return false;
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Format("{0} {1}", API.mod_name, ex));
				}
				return true;
			}

			private static void Postfix(bool __result, int _entityIdThatOpenedIt, TileEntity _te)
			{
				try
				{
                    if (_te != null)
					{
                        ClientInfo _cInfo = GetClient(_entityIdThatOpenedIt);
                        if (_cInfo != null)
                        {
                            if (!__result)
                            {
                                _cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageChat>().Setup(EChatType.Whisper, -1, Settings.DeniedMessage, null, EMessageSender.None));
                                CloseAllXui(_cInfo);
                            }
                        }
                    }
				}
                catch (Exception ex)
                {
                    Log.Error(string.Format("{0} {1}", API.mod_name, ex));
                }
            }

            private static ClientInfo GetClient(int _entityId)
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(_entityId);
			}

            private static bool CanOpenLootContainer(ClientInfo _cInfo, TileEntity _te)
			{
				try
				{
					if (_te != null)
					{
						if (_cInfo != null)
						{
							PersistentPlayerList ppl = GameManager.Instance.GetPersistentPlayerList();
							PersistentPlayerData ppd = ppl.GetPlayerData(_cInfo.InternalId);
							EntityPlayer _player = GameManager.Instance.World.Players.dict[_cInfo.entityId];
							Vector3i position = _te.ToWorldPos();
							if (GameManager.Instance.World.CanPlaceBlockAt(new Vector3i((int)position.x, (int)position.y, (int)position.z), ppd, true))
							{
								return true;
							}
						}
					}
				}
                catch (Exception ex)
                {
                    Log.Error(string.Format("{0} {1}", API.mod_name, ex));
                }
                return false;
			}

            private static void CloseAllXui(ClientInfo _cInfo)
			{
				try
				{
					_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageCloseAllWindows>().Setup(_cInfo.entityId));
				}
                catch (Exception ex)
                {
                    Log.Error(string.Format("{0} {1}", API.mod_name, ex));
                }
            }
		}
	}
}
