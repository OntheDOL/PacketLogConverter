using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x67, -1, ePacketDirection.ServerToClient, "Keep/Tower update")]
	public class StoC_0x67_KeepUpdate : Packet, IKeepIdPacket
	{
		protected ushort keepId;
		protected byte realm;
		protected byte level;
		protected byte count;
		protected byte[] components;
		protected byte unk1;

		/// <summary>
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		public ushort[] KeepIds
		{
			get { return new ushort[] { keepId }; }
		}

		public enum eComponentFlag: byte
		{
			RizedTower = 1, // only on towers
			HeightUpdate = 2,
			Broken = 4, // hole in wall
			AllowClimb = 8
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public byte Realm  { get { return realm; } }
		public byte Level { get { return level; } }
		public byte Count { get { return count; } }
		public byte[] Components { get { return components; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keepId:0x{0:X4} realm:{1} level:{2} count:{3}",
				keepId, realm, level, count);
			bool flagHeightUpdate = false;
			if (count > 0)
			{
				text.Write("  component flags:(");
				for (int i = 0; i < count; i++)
				{
					byte component = components[i];
					if (i > 0)
						text.Write(',');
					text.Write("0x{0:X2}", component);
					if (((component >> 4) & (int)eComponentFlag.HeightUpdate) == (int)eComponentFlag.HeightUpdate)
						flagHeightUpdate = true;
//					if (flagsDescription)
//						str.AppendFormat("({0})", (eComponentFlag)(component >> 4));
				}
				text.Write(")");
			}
			text.Write(" unk1:0x{0:X2}", unk1);
			if (flagsDescription)
			{
				if (flagHeightUpdate)
					text.Write(" (HeightUpdate)");
				else
					text.Write(" (Captured)");
			}
		}

		public override void Init()
		{
			Position = 0;
			keepId = ReadShort();         // 0x00
			realm = ReadByte();           // 0x02
			level = ReadByte();           // 0x03
			count = ReadByte();           // 0x04
			components = new byte[count];
			for (int i = 0; i < count; i++)
			{
				components[i] = ReadByte();
			}
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x67_KeepUpdate(int capacity) : base(capacity)
		{
		}
	}
}