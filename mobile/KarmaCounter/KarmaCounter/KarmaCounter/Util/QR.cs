using System;
using System.Threading.Tasks;

using ZXing;
using ZXing.Mobile;

namespace KarmaCounter.Util
{
    public class QR
    {
        public async Task<string> ScanAsync(Action callback)
        {
            MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions();
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();

            Result result = await scanner.Scan(options);
            callback();

            return result == null ? "" : result.Text;
        }
    }
}
