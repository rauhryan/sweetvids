module Synthesis
  module AssetPackageHelper
    
    def should_merge?
      AssetPackage.merge_environments.include?(Rails.env)
    end

    def javascript_include_merged(*sources)
      options = sources.last.is_a?(Hash) ? sources.pop.stringify_keys : { }

      if sources.include?(:defaults) 
        sources = sources[0..(sources.index(:defaults))] + 
          (File.exists?("#{FubuRoot}/#{FubuWebProject}/content/scripts/application.js") ? ['application'] : []) + 
          sources[(sources.index(:defaults) + 1)..sources.length]
        sources.delete(:defaults)
      end

      sources.collect!{|s| s.to_s}
      sources = (should_merge? ? 
        AssetPackage.targets_from_sources("scripts", sources) : 
        AssetPackage.sources_from_targets("scripts", sources))
        
      sources.collect {|source| javascript_include_tag(source, options) }.join("\n")
    end

    def stylesheet_link_merged(*sources)
      options = sources.last.is_a?(Hash) ? sources.pop.stringify_keys : { }

      sources.collect!{|s| s.to_s}
      sources = (should_merge? ? 
        AssetPackage.targets_from_sources("css", sources) : 
        AssetPackage.sources_from_targets("css", sources))

      sources.collect { |source| stylesheet_link_tag(source, options) }.join("\n")    
    end

  end
end