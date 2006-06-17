using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 180, ePacketDirection.ClientToServer, "Region list request v180")]
	public class CtoS_0x9D_RegionListRequest_180 : CtoS_0x9D_RegionListRequest_174
	{
		protected uint VedioVendorId1;
		protected uint VedioVendorId2;

		public override string GetPacketDataString(bool flagsDescription)
		{
			string str = base.GetPacketDataString(flagsDescription);
			if (flag > 0)
				str += "\n\tnew in 1.80 VideoCard VendorId:0x" + VedioVendorId1.ToString("X8") + "(0x" + VedioVendorId2.ToString("X8") + ")";
			return str;
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			slot = ReadByte();
			flag = ReadByte();
			if (flag > 0)
			{
				resolution = ReadShort();
				options = ReadShort();
				unk1 = ReadInt();
				figureVersion = ReadInt();
				figureVersion1 = ReadByte();
				skin = ReadByte();
				race = ReadByte();
				regionExpantions = ReadByte();
				classId = ReadByte();
				expantions = ReadByte();
				VedioVendorId1 = ReadIntLowEndian();
				VedioVendorId2 = ReadIntLowEndian();
				zero = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_180(int capacity) : base(capacity)
		{
		}
	}
}