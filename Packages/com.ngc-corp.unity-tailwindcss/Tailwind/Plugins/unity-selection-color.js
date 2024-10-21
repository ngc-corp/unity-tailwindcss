plugin(function ({ addUtilities, theme }) {
  const colors = theme('colors');
  const newUtilities = {};

  // Loop through all the colors defined in the Tailwind config
  Object.keys(colors).forEach(colorName => {
    const colorValue = colors[colorName];

    // Add a utility class for each color to set --unity-selection-color
    newUtilities[`.u-selection-${colorName}`] = {
      '--unity-selection-color': colorValue,
    };
  });

  // Register the new utilities with Tailwind
  addUtilities(newUtilities, ['responsive', 'hover', 'focus', 'active']);
})
