plugin(function ({ addUtilities, theme }) {
  const colors = theme('colors');
  const opacityValues = theme('opacity');
  const utilities = {};

  // Function to convert hex to RGB
  const hexToRgb = (hex) => {
    let r, g, b;
    if (hex.length === 4) {
      r = parseInt(hex[1] + hex[1], 16);
      g = parseInt(hex[2] + hex[2], 16);
      b = parseInt(hex[3] + hex[3], 16);
    } else if (hex.length === 7) {
      r = parseInt(hex[1] + hex[2], 16);
      g = parseInt(hex[3] + hex[4], 16);
      b = parseInt(hex[5] + hex[6], 16);
    }
    return [r, g, b];
  };

  // Recursive function to handle nested color objects
  const handleColor = (colorKey, colorValue) => {
    // Check if the color value is an object (e.g., { '50': '#123456', '100': '#234567' })
    if (typeof colorValue === 'object') {
      Object.keys(colorValue).forEach((shade) => {
        handleColor(`${colorKey}-${shade}`, colorValue[shade]);
      });
    } else {
      // If it's a string, process the color
      Object.keys(opacityValues).forEach((opacityKey) => {
        const opacityValue = opacityValues[opacityKey];

        // Handle hex colors
        if (typeof colorValue === 'string' && colorValue.startsWith('#')) {
          const [r, g, b] = hexToRgb(colorValue);

          utilities[`.bg-${colorKey}-opacity-${opacityKey}`] = {
            backgroundColor: `rgba(${r}, ${g}, ${b}, ${opacityValue})`,
          };
          utilities[`.border-${colorKey}-opacity-${opacityKey}`] = {
            borderColor: `rgba(${r}, ${g}, ${b}, ${opacityValue})`,
          };
          utilities[`.text-${colorKey}-opacity-${opacityKey}`] = {
            color: `rgba(${r}, ${g}, ${b}, ${opacityValue})`,
          };
        }

        // Handle predefined keywords: transparent, inherit, currentColor
        else if (['inherit', 'currentColor', 'transparent'].includes(colorValue)) {
          utilities[`.bg-${colorKey}-opacity-${opacityKey}`] = {
            backgroundColor: `${colorValue}`,
          };
          utilities[`.border-${colorKey}-opacity-${opacityKey}`] = {
            borderColor: `${colorValue}`,
          };
          utilities[`.text-${colorKey}-opacity-${opacityKey}`] = {
            color: `${colorValue}`,
          };
        }
      });
    }
  };

  // Loop through all colors and process them recursively
  Object.keys(colors).forEach((colorKey) => {
    const colorValue = colors[colorKey];
    handleColor(colorKey, colorValue);
  });

  // Add the utilities (responsive, hover, focus, etc.)
  addUtilities(utilities, ['responsive', 'hover', 'focus', 'active']);
})
