using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF6, -1, ePacketDirection.ServerToClient, "Change target")]
	public class StoC_0xF6_ChangeTarget : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected byte type;
		protected byte unk1; // unused

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public byte Type { get { return type; } }
		public byte Unk1 { get { return unk1; } }

		public enum eChangeTargetType: byte
		{
			SetTargetOidAssist = 0, // assist
			SetCheckTargetOid = 1,
			SetTargetOid = 2,
			SetCombatMode = 3, // if !Combat
		}

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("oid:0x{0:X4} type:{1}", oid, type, unk1);
			if (flagsDescription)
			{
				text.Write("({0})", (eChangeTargetType)type);
//				text.Write(" unk1:0x{0:X2}", unk1);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			oid = ReadShort();
			type = ReadByte();
			unk1 = ReadByte(); // unused
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF6_ChangeTarget(int capacity) : base(capacity)
		{
		}
	}
}