using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x66, 174, ePacketDirection.ServerToClient, "Warmap bonuses v174")]
	public class StoC_0x66_WarmapBonuses_174 : StoC_0x66_WarmapBonuses
	{
		protected byte towers;
		protected byte ownerDFtowers;

		#region public access properties

		public byte Towers { get { return towers; } }
		public byte OwnerDFtowers { get { return ownerDFtowers; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keeps:{0} relics:0x{1:X2}(power:{5} strength:{6}) towers:{2} DFownerRealm:{3} DFownerTowers:{4}",
				keeps, relics, towers, ownerDFrealm, ownerDFtowers, relics >> 4, relics & 0xF);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			base.Init();

			towers = ReadByte();        // 0x03
			ownerDFtowers = ReadByte(); // 0x04
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x66_WarmapBonuses_174(int capacity) : base(capacity)
		{
		}
	}
}