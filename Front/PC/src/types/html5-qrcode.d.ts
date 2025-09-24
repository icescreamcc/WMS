// src/types/html5-qrcode.d.ts
declare module "html5-qrcode" {
  export class Html5Qrcode {
    constructor(elementId: string);
    start(
      cameraIdOrConfig: any,
      config: any,
      qrCodeSuccessCallback: (decodedText: string, decodedResult: any) => void,
      qrCodeErrorCallback?: (errorMessage: string) => void
    ): Promise<void>;
    stop(): Promise<void>;
    clear(): Promise<void>;
    scanFileV2(file: File, showImage?: boolean): Promise<{ decodedText: string }>;
  }

  export class Html5QrcodeScanner {
    constructor(
      elementId: string,
      config: any,
      verbose: boolean
    );
    render(
      qrCodeSuccessCallback: (decodedText: string, decodedResult: any) => void,
      qrCodeErrorCallback?: (errorMessage: string) => void
    ): void;
    clear(): Promise<void>;
  }

  export enum Html5QrcodeSupportedFormats {
    QR_CODE = 0,
    AZTEC = 1,
    CODABAR = 2,
    CODE_39 = 3,
    CODE_93 = 4,
    CODE_128 = 5,
    DATA_MATRIX = 6,
    MAXICODE = 7,
    ITF = 8,
    EAN_13 = 9,
    EAN_8 = 10,
    PDF_417 = 11,
    RSS_14 = 12,
    RSS_EXPANDED = 13,
    UPC_A = 14,
    UPC_E = 15,
    UPC_EAN_EXTENSION = 16
  }
}
