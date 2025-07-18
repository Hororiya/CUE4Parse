using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine.Curves;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.MovieScene;

public readonly struct FMovieSceneValue<T> : IUStruct
{
    public readonly T Value;
    public readonly FMovieSceneTangentData Tangent;
    public readonly ERichCurveInterpMode InterpMode;
    public readonly ERichCurveTangentMode TangentMode;

    public FMovieSceneValue(FAssetArchive Ar, T value, bool compact = false)
    {
        Value = value;
        if (FSequencerObjectVersion.Get(Ar) < FSequencerObjectVersion.Type.SerializeFloatChannelCompletely)
        {
            InterpMode = Ar.Read<ERichCurveInterpMode>();
            TangentMode = Ar.Read<ERichCurveTangentMode>();
            Tangent = new FMovieSceneTangentData(Ar);
        }
        else
        {
            if (compact)
            {
                Tangent = new FMovieSceneTangentData(Ar.Read<float>(), Ar.Read<float>(), Ar.Read<float>(), Ar.Read<float>(), Ar.Read<ERichCurveTangentWeightMode>());
                InterpMode = Ar.Read<ERichCurveInterpMode>();
                TangentMode = Ar.Read<ERichCurveTangentMode>();
                Ar.Position += 1; // Padding
            }
            else
            {
                Tangent = new FMovieSceneTangentData(Ar);
                InterpMode = Ar.Read<ERichCurveInterpMode>();
                TangentMode = Ar.Read<ERichCurveTangentMode>();
                Ar.Position += 2; // Padding
            }
        }
    }
}
