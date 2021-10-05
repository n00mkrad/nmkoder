# Nmkoder
Video encoding, muxing, and analysis GUI in Winforms, built around FFmpeg, FFprobe, and av1an.

![](https://i.imgur.com/c8XtSlG.png)



## Features

#### Input

- Supports all formats that ffmpeg can decode
- Either use **"Multi File Mode"** to merge multiple files into one, or **"Batch Processing Mode"** to run an action on each file 
- Supports image sequence inputs (PNG/WEBP/JPEG/BMP) without requiring sequential filenames (FPS needs to be set manually)

#### Track List

- View codec, language, title and more (depending on stream type) of selected media stream
- Enable or disable streams with checkboxes - Disabled streams will not be included when encoding/muxing
- Re-order streams

#### Convert (FFmpeg)

- Encode video using ffmpeg and its encoder plugins
- Video Formats: **H264 (x264 or NVENC), H265 (x265 or NVENC), VP9, AV1**
- Image Formats: Animated **GIF**, **PNG** Sequence, **JPEG** Sequence
- Audio Formats: **AAC, Opus, MP3, FLAC**
- Subtitle Formats: Mov_Text for MP4 and MOV, SRT for MKV, WebVTT for WEBM
- All media types also have the option to **strip** (remove) or **copy** (mux without re-encoding) instead of encoding
- Set metadata (title and language) for each track
- Encoder Options: Set quality and speed/effort aka preset, set color format
- Quality Modes: Either use a **constant quality**, **target bitrate**, or target filesize
- Video Options: Resample frame rate, **resize** either using absolute or relative numbers, **automatically crop** black bars
- Audio Options: Set quality and channels/layout
- Subtitle Options: Optionally **burn in** a subtitle track

#### AV1AN Chunked Encoding

- Encode video using [av1an](https://github.com/master-of-zen/Av1an) and supported encoders
- Video Formats: **H265 (x265), VP9 (VPX), AV1 (AOM or SVT-AV1)**
- Quality Modes: Either use a **constant quality** or target a **VMAF** score
- Same audio and video options as FFmpeg encoding
- Set AV1 film **grain synthesis** (disabled for H265/VP9 as this is exclusive to AV1)
- Av1an Options: Change splitting method, chunk creation method, amount of workers, and more

#### Utilities

- Utilities are "shortcuts" for actions that normally require long (and/or multiple) CLI commands
- Read Bitrates: Calculates stream size and average bitrate for each stream
- Get Metrics: Calculate quality metrics like **VMAF**, SSIM, PSNR

## Compatibility

- Tested on and intended for Windows 10 64-bit. Windows 11 should work as well.
- .NET Framework 4.7.2 is required which should be pre-installed.
- There is no native Linux or Mac port and I don't plan on making one as Linux users constantly tell me how much they love their CLI. However, if you still want to run this on Linux, use WINE, it should work without problems. 
