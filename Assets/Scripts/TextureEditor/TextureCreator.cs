using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class TextureCreator : MonoBehaviour
{
	// Add your own pattern types here:
	public enum PatternType
	{
		Noise,
		GradientHorizontal,
		GradientVertical,
		GradientDiagonal,
		GradientDiagonalColored,
		Stripes,
		CheckerBoard,
		StripesDiagonal,
		CircleFilled,
		Dot,
		Circle,
		Sides,
		Cos,
		PerlinNoise,
		Wood,
		ColourPicker,
		ColourPicker2,
		OverlapSin,
		OverlapSin2,
		None,
		Mandelbrot
	};
	public PatternType patternType;

	const int SIZE = 1024;

	public Texture2D texture = null;
	Color[] pixelArr = null;

	void Start()
	{
		// Create a texture and pass it to the material of this game object's renderer:
		Renderer rend = GetComponent<Renderer>();
		texture = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);
		rend.material.mainTexture = texture;
		texture.wrapMode = TextureWrapMode.Clamp;

		Draw();
	}

	/// <summary>
	/// Returns the pixel color for texture coordinate (u,v), for a given pattern.
	/// </summary>
	Color CalculatePixelColor(float u, float v, PatternType pattern)
	{
		return pattern switch
		{
			PatternType.Noise => Random.value * Color.white,
			PatternType.GradientHorizontal => Color.white * u,
			PatternType.GradientVertical => Color.white * v,
			PatternType.GradientDiagonal => Color.white * Mathf.Max(u, v),
			PatternType.GradientDiagonalColored => Color.red * u + Color.green * v,
			PatternType.Stripes => u % 0.2 > 0.1 ? Color.white : Color.black,
			PatternType.CheckerBoard => u % 0.2 > 0.1 != v % 0.2 > 0.1 ? Color.white : Color.black,
			PatternType.StripesDiagonal => Color.Lerp(Color.yellow, Color.green * 0.5f, (Mathf.Cos(-45f * Mathf.Deg2Rad) * (u + v) % 0.1f) + 0.2f),
			PatternType.CircleFilled => CircleFilled(u, v),
			PatternType.Dot => Dot(u, v),
			PatternType.Circle => Circle(u, v),
			PatternType.Sides => Sides(u),
			PatternType.Cos => Cos(u, v),
			PatternType.PerlinNoise => Color.white * Mathf.PerlinNoise(u * 10, v * 10),
			PatternType.Wood => Wood(u, v),
			PatternType.ColourPicker => ColourPicker(u, v),
			PatternType.ColourPicker2 => ColourPicker2(u, v),
			PatternType.OverlapSin => OverlapSin(u, v),
			PatternType.OverlapSin2 => OverlapSin2(u, v),
			PatternType.Mandelbrot => Mandelbrot(3 * (u - 0.75f), 3 * (v - 0.5f)),
			_ => Color.blue,
		};
	}

	Color ColourPicker2(float u, float v)
	{
		float cos1 = (Mathf.Cos(Mathf.Lerp(1, 0, u) * Mathf.PI * 2) + 1) / 2;
		float cos2 = (Mathf.Cos(Mathf.Lerp(1, 0, u) * Mathf.PI * 2 - Mathf.PI / 1.5f) + 1) / 2;
		float cos3 = (Mathf.Cos(Mathf.Lerp(1, 0, u) * Mathf.PI * 2 + Mathf.PI / 1.5f) + 1) / 2;

		return new(cos1, cos2, cos3);
	}

	Color OverlapSin2(float u, float v)
	{
		float cos1 = (Mathf.Cos(u * Mathf.PI * 2) + 1) / 2;
		float cos2 = (Mathf.Cos(u * Mathf.PI * 2 - Mathf.PI / 1.5f) + 1) / 2;
		float cos3 = (Mathf.Cos(u * Mathf.PI * 2 + Mathf.PI / 1.5f) + 1) / 2;

		Color col1 = Color.red;
		Color col2 = Color.blue;
		Color col3 = Color.green;

		Color color = new();
		if (v < cos1)
			color += col1;
		if (v < cos2)
			color += col2;
		if (v < cos3)
			color += col3;

		return color;
	}

	Color ColourPicker(float u, float v)
	{
		float cos1 = (Mathf.Cos(Mathf.Lerp(1, 0, u) * Mathf.PI * 2) + 1) / 2;
		float cos2 = (Mathf.Cos(Mathf.Lerp(1, 0, u) * Mathf.PI * 2 - Mathf.PI / 1.5f) + 1) / 2;
		float cos3 = (Mathf.Cos(Mathf.Lerp(1, 0, u) * Mathf.PI * 2 + Mathf.PI / 1.5f) + 1) / 2;

		return new(cos1, cos2, cos3);
	}

	Color OverlapSin(float u, float v)
	{
		float cos1 = (Mathf.Cos(u * Mathf.PI * 2) + 1) / 2;
		float cos2 = (Mathf.Cos(u * Mathf.PI * 2 - Mathf.PI / 1.5f) + 1) / 2;
		float cos3 = (Mathf.Cos(u * Mathf.PI * 2 + Mathf.PI / 1.5f) + 1) / 2;

		Color col1 = Color.red;
		Color col2 = Color.blue;
		Color col3 = Color.green;

		Color color = new();
		if (v < cos1)
			color += col1;
		if (v < cos2)
			color += col2;
		if (v < cos3)
			color += col3;

		return color;
	}

	Color Wood(float u, float v)
	{
		Color lightBrown = new(0.7f, 0.5f, 0.3f);
		Color brown = new(0.4f, 0.25f, 0.1f);

		float perlin = Mathf.PerlinNoise(u * 200, v * 10);

		return Color.Lerp(lightBrown, brown, perlin);
	}

	Color Cos(float u, float v)
	{
		float cos = (Mathf.Cos(u * Mathf.PI * 2) + 1) / 2;

		return v < cos ? Color.red : Color.black;
	}

	Color Sides(float u)
	{
		float cos = Mathf.Cos(u * Mathf.PI * 2);

		return cos > 0.2f
			? Color.Lerp(Color.black, Color.red, cos)
			: Color.black;
	}

	Color Circle(float u, float v)
	{
		float distanceX = u - 0.5f;
		float distanceY = v - 0.5f;

		// pythagoras for finding sitance from center from uv cords
		float dist = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

		// circle edge
		float inner = 0.20f;
		float outer = 0.30f;

		if (dist < inner || dist > outer)
			return Color.black;

		// center of the ring
		float ringCenter = (inner + outer) * 0.5f;

		// distance from brightest point
		float distFromCenter = Mathf.Abs(dist - ringCenter);

		// normalize glow
		float t = distFromCenter / ((outer - inner) * 0.5f);

		// invert so center is brightest
		t = 1f - t;

		// sharpen glow
		t *= t;

		Color neon = new(1f, 0f, 1f);

		return Color.Lerp(Color.black, neon, t);
	}

	Color Dot(float u, float v)
	{
		float diffx = Mathf.Abs(u - 0.5f);
		float diffy = Mathf.Abs(v - 0.5f);

		float distance = Mathf.Sqrt((diffx * diffx) + (diffy * diffy));

		bool inCircle = distance < 0.3f;

		return distance < 0.3f ? Color.Lerp(Color.magenta, Color.black, Mathf.InverseLerp(0f, 0.3f, distance)) : Color.black;
	}

	Color CircleFilled(float u, float v)
	{
		float diffx = Mathf.Abs(u - 0.5f);
		float diffy = Mathf.Abs(v - 0.5f);

		float distance = Mathf.Sqrt((diffx * diffx) + (diffy * diffy));

		return distance < 0.3f ? Color.red : Color.black;
	}

	/// <summary>
	/// Draws a pattern given by the [pattern] number to the [cols] array, which
	/// should have size [width] * [height].
	/// </summary>
	void DrawPattern(Color[] pixelArr, int width, int height, PatternType pattern)
	{
		for (int index = 0; index < width * height; index++)
		{

			// int index = y * width + x
			int y = index / width; // floor of outcome is automaticly with int in c#
			int x = index % width; // 

			float u = (float)x / width;
			float v = (float)y / height;

			pixelArr[index] = CalculatePixelColor(u, v, pattern);
		}
	}

	void Draw()
	{
		pixelArr ??= texture.GetPixels();
		DrawPattern(pixelArr, SIZE, SIZE, patternType);

		texture.SetPixels(pixelArr);
		texture.Apply();
	}

	// OnValidate is called whenever an inspector value is changed - even in edit mode!
	void OnValidate()
	{
		// To prevent calling Draw code in edit mode,
		// we check whether a texture has been created (in Start)
		if (texture == null) return;
		Draw();
	}

	[ContextMenu("Save File")]
	public void OnSaveFile() {
        var exporter = GetComponent<TextureExporter>();
        if (exporter != null) {
            exporter.ExportTexture(texture);
        }
    }

	private void Update()
	{
	}

	#region Mandelbrot
	// Used for the Mandelbrot fractal:
	const int maxIterations = 30;
	const float escapeLengthSquared = 4;

	Color Mandelbrot(float cReal, float cImaginary)
	{
		int iteration = 0;

		float zReal = 0;
		float zImaginary = 0;

		while (zReal * zReal + zImaginary * zImaginary < escapeLengthSquared && iteration < maxIterations)
		{
			// Use Mandelbrot's magic iteration formula: z := z^2 + c 
			// (using complex number multiplication & addition - 
			//   see https://mathbitsnotebook.com/Algebra2/ComplexNumbers/CPArithmeticASM.html)
			float newZr = zReal * zReal - zImaginary * zImaginary + cReal;
			zImaginary = 2 * zReal * zImaginary + cImaginary;
			zReal = newZr;
			iteration++;
		}
		// Return a color value based on the number of iterations that were needed to "escape the circle":
		float grad = 1f * iteration / maxIterations; // between 0 and 1
													 // TODO: use a nicer gradient
		return new Color(grad, grad, grad);
	}
	#endregion
}
